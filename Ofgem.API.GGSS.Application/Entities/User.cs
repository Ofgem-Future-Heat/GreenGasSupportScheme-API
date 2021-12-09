using Ofgem.API.GGSS.Application.Json;
using Ofgem.API.GGSS.Domain.Contracts.Entities;
using Ofgem.API.GGSS.Domain.ModelValues;
using System;
using System.Collections.Generic;

namespace Ofgem.API.GGSS.Application.Entities
{
    public class User : IDbEntity, ISerializableEntity<UserValue>
    {
        public Guid Id { get; set; }
        public string ProviderId { get; set; }
        public ICollection<ResponsiblePerson> ResponsiblePeople { get; set; }
        
        public ICollection<UserOrganisation> UserOrganisations { get; set; }

        public bool IsResponsiblePerson { get; set; }

        public User() : this(new UserValue()) { }

        public User(UserValue value)
        {
            this.Value = value;
            this.ResponsiblePeople = new HashSet<ResponsiblePerson>();
            this.UserOrganisations = new HashSet<UserOrganisation>();
        }

        #region Value

        private JsonField<UserValue> _value;

        // Used by EF
        public string Json
        {
            get { return _value.Json; }
            set { _value.Json = value; }
        }

        // Used by application code
        public UserValue Value
        {
            get { return _value.Object; }
            set { _value.Object = value; }
        }

        #endregion Value
    }
}
