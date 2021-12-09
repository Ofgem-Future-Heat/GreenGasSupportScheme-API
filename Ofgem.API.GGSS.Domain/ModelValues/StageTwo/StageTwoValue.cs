namespace Ofgem.API.GGSS.Domain.ModelValues.StageTwo
{
    public class StageTwoValue
    {
        public Isae3000Value Isae3000 { get; set; } = new Isae3000Value();
        public AdditionalSupportingEvidenceValue AdditionalSupportingEvidence { get; set; } = new AdditionalSupportingEvidenceValue();
    }
}