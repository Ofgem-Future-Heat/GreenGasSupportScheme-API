using Ofgem.API.GGSS.Application.Json;
using Ofgem.API.GGSS.Domain.Contracts.Entities;
using Ofgem.API.GGSS.Domain.ModelValues;
using System;
using System.Collections.Generic;

namespace Ofgem.API.GGSS.Application.Entities
{
    public class Organisation : IDbEntity, ISerializableEntity<OrganisationValue>
    {
        public Guid Id { get; set; }
        public ICollection<ResponsiblePerson> ResponsiblePeople { get; set; }
        
        public ICollection<UserOrganisation> UserOrganisations { get; set; }
        public ICollection<Application> Applications { get; set; }

        public Organisation()
        {
            this.Applications = new HashSet<Application>();
            this.ResponsiblePeople = new HashSet<ResponsiblePerson>();
            this.UserOrganisations = new HashSet<UserOrganisation>();
        }

        #region Value

        private JsonField<OrganisationValue> _value;

        // Used by EF
        public string Json
        {
            get { return _value.Json; }
            set { _value.Json = value; }
        }

        // Used by application code
        public OrganisationValue Value
        {
            get { return _value.Object; }
            set { _value.Object = value; }
        }

        #endregion Value
    }
}
