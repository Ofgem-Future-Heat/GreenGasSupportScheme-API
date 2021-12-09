namespace Ofgem.API.GGSS.Domain.ModelValues.StageOne
{
    public class StageOneValue
    {
        public TellUsAboutYourSiteValue TellUsAboutYourSite { get; set; } = 
            new TellUsAboutYourSiteValue();

        public ProvidePlanningPermissionValue ProvidePlanningPermission { get; set; } =
            new ProvidePlanningPermissionValue();

        public ProductionDetailsValue ProductionDetails { get; set; } =
            new ProductionDetailsValue();

    }
}