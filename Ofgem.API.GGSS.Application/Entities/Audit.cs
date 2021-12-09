using System;

namespace Ofgem.API.GGSS.Application.Entities
{
    public class Audit
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid EntityId { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string AffectedColumns { get; set; }
        public string PrimaryKey { get; set; }
    }
}
