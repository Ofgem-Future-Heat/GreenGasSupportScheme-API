using Ofgem.API.GGSS.Application.Json;
using Ofgem.API.GGSS.Domain.Contracts.Entities;
using Ofgem.API.GGSS.Domain.ModelValues;
using System;
using System.Collections.Generic;

namespace Ofgem.API.GGSS.Application.Entities
{
    public class UserOrganisation : IDbEntity
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public User User { get; set; }
        public Guid OrganisationId { get; set; }
        public Organisation Organisation { get; set; }
        public string InvitedEmail { get; set; }
    }
}
