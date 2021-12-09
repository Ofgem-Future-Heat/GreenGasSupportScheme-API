using Ofgem.API.GGSS.Application.Json;
using Ofgem.API.GGSS.Domain.Contracts.Entities;
using Ofgem.API.GGSS.Domain.ModelValues;
using System;
using System.Collections.Generic;

namespace Ofgem.API.GGSS.Application.Entities
{
    public partial class Application : IDbEntity, ISerializableEntity<ApplicationValue>
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public Guid OrganisationId { get; set; }
        public Organisation Organisation { get; set; }
        public ICollection<Document> Documents { get; set; }

        public Application() : this(new ApplicationValue()) { }

        public Application(ApplicationValue value)
        {
            this.Value = value;
            this.Documents = new HashSet<Document>();
        }

        #region Value

        private JsonField<ApplicationValue> _value;

        // Used by EF
        public string Json
        {
            get { return _value.Json; }
            set { _value.Json = value; }
        }

        // Used by application code
        public ApplicationValue Value
        {
            get { return _value.Object; }
            set { _value.Object = value; }
        }

        #endregion Value
    }
}
