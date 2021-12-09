using Ofgem.API.GGSS.Domain.Models;

namespace Ofgem.API.GGSS.Domain.ModelValues.StageOne
{
    public class ProvidePlanningPermissionValue
    {
        public string Status { get; set; } = "NotStarted";
        public string PlanningPermissionOutcome { get; set; }
        public DocumentValue PlanningPermissionDocument { get; set; } = new DocumentValue();
        public string PlanningPermissionStatement { get; set; }
    }
}