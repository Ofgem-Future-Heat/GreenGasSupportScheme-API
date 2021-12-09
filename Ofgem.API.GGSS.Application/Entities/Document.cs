using Ofgem.API.GGSS.Application.Json;
using Ofgem.API.GGSS.Domain.Contracts.Entities;
using Ofgem.API.GGSS.Domain.ModelValues;
using System;

namespace Ofgem.API.GGSS.Application.Entities
{
    public class Document : IDbEntity, ISerializableEntity<DocumentValue>
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public Application Application { get; set; }

        public Document() : this(new DocumentValue()) { }

        public Document(DocumentValue value)
        {
            this.Value = value;
        }

        #region Value

        private JsonField<DocumentValue> _value;

        // Used by EF
        public string Json
        {
            get { return _value.Json; }
            set { _value.Json = value; }
        }

        // Used by application code
        public DocumentValue Value
        {
            get { return _value.Object; }
            set { _value.Object = value; }
        }

        #endregion Value
    }
}
