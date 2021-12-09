using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.ModelValues;
using System.Collections.Generic;

namespace Ofgem.API.GGSS.Domain.Responses.Applications
{
    public class ApplicationResponse : BaseResponse
    {
        public string Id { get; set; }
        public ApplicationResponse() : base() { }
        public ApplicationValue Value { get; set; }
        public List<DocumentModel> Documents { get; set; }
    }
}
