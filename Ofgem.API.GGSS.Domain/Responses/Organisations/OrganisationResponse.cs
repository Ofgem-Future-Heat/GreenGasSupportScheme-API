namespace Ofgem.API.GGSS.Domain.Responses.Organisations
{
    public class OrganisationResponse : BaseResponse
    {
        public string Id { get; set; }
        public string ReferenceNumber { get; set; }
        public OrganisationResponse() : base() { }
    }
}
