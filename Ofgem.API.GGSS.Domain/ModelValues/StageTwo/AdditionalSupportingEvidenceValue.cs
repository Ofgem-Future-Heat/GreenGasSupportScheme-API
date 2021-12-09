using System.Collections.Generic;

namespace Ofgem.API.GGSS.Domain.ModelValues.StageTwo
{
    public class AdditionalSupportingEvidenceValue
    {
        public string Status { get; set; } = "CannotStartYet";
        public List<DocumentValue> AdditionalSupportingEvidenceDocuments { get; set; } = new List<DocumentValue>();
        public string AddEvidence { get; set; }
    }
}