namespace Ofgem.API.GGSS.Domain.ModelValues
{
    public class ResponsiblePersonValue
    {
        public string TelephoneNumber { get; set; }

        public string DateOfBirth { get; set; }
        public DocumentValue PhotoId { get; set; }
        public DocumentValue BankStatement { get; set; }
        public DocumentValue LetterOrAuthority { get; set; }
    }
}
