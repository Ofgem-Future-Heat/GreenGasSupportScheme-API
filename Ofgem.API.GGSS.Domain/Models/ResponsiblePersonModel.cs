using Ofgem.API.GGSS.Domain.ModelValues;

namespace Ofgem.API.GGSS.Domain.Models
{
    public class ResponsiblePersonModel : ResponsiblePersonNomineeModel
    {
        public string Id { get; set; }
        public UserModel User { get; set; }
        public DocumentValue PhotographicIdFile { get; set; }
        public DocumentValue BankStatementOrBillFile { get; set; }
    }
}
