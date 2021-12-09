namespace Ofgem.API.GGSS.Domain.ModelValues.StageTwo
{
    public class Isae3000Value
    {
        public string Status { get; set; } = "CannotStartYet";
        public DocumentValue Document { get; set; } = new DocumentValue();
    }
}