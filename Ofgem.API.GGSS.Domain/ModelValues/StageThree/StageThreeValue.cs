namespace Ofgem.API.GGSS.Domain.ModelValues.StageThree
{
    public class StageThreeValue
    {
        public OrganisationDetailsValue OrganisationDetails { get; set; } 
            = new OrganisationDetailsValue();
        
        public FeedstockDetailsValue FeedstockDetails { get; set; } =
            new FeedstockDetailsValue();
    }
}
