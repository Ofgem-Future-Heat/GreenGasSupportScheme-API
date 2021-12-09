using Ofgem.API.GGSS.Application.Json;
using Ofgem.API.GGSS.Domain.Contracts.Entities;
using Ofgem.API.GGSS.Domain.ModelValues;
using System;
using System.Collections.Generic;

namespace Ofgem.API.GGSS.Application.Entities
{
    public class ResponsiblePerson : IDbEntity, ISerializableEntity<ResponsiblePersonValue>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid OrganisationId { get; set; }
        public Organisation Organisation { get; set; }
    
        public ICollection<Document> Documents { get; set; }

        public ResponsiblePerson()
        {
            this.Documents = new HashSet<Document>();
        }

        #region Value

        private JsonField<ResponsiblePersonValue> _value;

        // Used by EF
        public string Json
        {
            get { return _value.Json; }
            set { _value.Json = value; }
        }

        // Used by application code
        public ResponsiblePersonValue Value
        {
            get { return _value.Object; }
            set { _value.Object = value; }
        }

        #endregion Value
    }
}
