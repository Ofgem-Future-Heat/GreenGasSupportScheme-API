using Ofgem.API.GGSS.Domain.Models;

namespace Ofgem.API.GGSS.Domain.ModelValues.StageOne
{
    public class TellUsAboutYourSiteValue
    {
        public string PlantName { get; set; }
        public string Status { get; set; } = "NotStarted";
        public string PlantLocation { get; set; }
        public LatitudeLongitudeValue LatitudeLongitudeAnaerobic { get; set; } = new LatitudeLongitudeValue();
        public LatitudeLongitudeValue LatitudeLongitudeInjection { get; set; } = new LatitudeLongitudeValue();
        public DocumentValue CapacityCheckDocument { get; set; } = new DocumentValue();
        public AddressModel PlantAddress { get; set; } = new AddressModel();
        public AddressModel InjectionPointAddress { get; set; } = new AddressModel();
        public string EquipmentDescription { get; set; }
        public string HasPostcode { get; set; }
    }
}