using Ofgem.API.GGSS.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Ofgem.API.GGSS.Persistence.Auditing;
using System.Collections.Generic;
using System.Threading;

using Models = Ofgem.API.GGSS.Domain.Models;
using Values = Ofgem.API.GGSS.Domain.ModelValues;
using Entities = Ofgem.API.GGSS.Application.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace Ofgem.API.GGSS.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggedInUserService loggedInUserService)
            : base(options) { }

        public DbSet<Entities.User> Users { get; set; }
        public DbSet<Entities.Organisation> Organisations { get; set; }
        public DbSet<Entities.ResponsiblePerson> ResponsiblePeople { get; set; }
        public DbSet<Entities.Application> Applications { get; set; }
        public DbSet<Entities.Document> Documents { get; set; }
        public DbSet<Entities.Audit> AuditLogs { get; set; }

        public DbSet<Entities.UserOrganisation> UserOrganisations { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Ignore<Entities.Address>();

            modelBuilder.Ignore<Models.AddressModel>();

            modelBuilder.Ignore<Values.UserValue>();
            modelBuilder.Ignore<Values.OrganisationValue>();
            modelBuilder.Ignore<Values.ApplicationValue>();
            modelBuilder.Ignore<Values.DocumentValue>();
            modelBuilder.Ignore<Values.ResponsiblePersonValue>();
        }

        public virtual async Task<int> SaveChangesAsync(Guid userId = default, CancellationToken token = default)
        {
            OnBeforeSaveChanges(userId);

            return await base.SaveChangesAsync(token);
        }

        private void OnBeforeSaveChanges(Guid userId)
        {
            var changes = FindChanges(userId, out Guid entityId);

            changes.ForEach(c => AuditLogs.Add(c.ToAudit(entityId)));
        }

        private List<AuditEntry> FindChanges(Guid userId, out Guid entityId)
        {
            entityId = Guid.Empty;

            ChangeTracker.DetectChanges();

            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (IsNotTracked(entry))
                {
                    continue;
                }

                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Entity.GetType().Name,
                    UserId = userId
                };

                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;

                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        Guid.TryParse(property.CurrentValue.ToString(), out entityId);
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }

            return auditEntries;
        }

        private bool IsNotTracked(EntityEntry entry)
        {
            return entry.Entity is Entities.Audit
                || entry.State == EntityState.Detached
                || entry.State == EntityState.Unchanged;
        }
    }
}
