using Ofgem.API.GGSS.Domain.Contracts.Entities;
using Ofgem.API.GGSS.Domain.ModelValues;

namespace Ofgem.API.GGSS.Domain.Models
{
    public class DocumentModel : ISerializableEntity<DocumentValue>
    {
        public string Id { get; set; }
        public DocumentValue Value { get; set; }
    }
}
