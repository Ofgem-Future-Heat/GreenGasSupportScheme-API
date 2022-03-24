using Ofgem.API.GGSS.Domain.Enums;

namespace Ofgem.API.GGSS.Application.Helpers
{
    public static class MapApplicationStatusHelper
    {
        public static ApplicationStatus GetMappedStatus(Entities.Application application)
        {
            if (IsNotRejected(application))
            {
                return application.Value.Status;
            }
            
            return IsStageOneRejected(application) ? ApplicationStatus.StageOneRejected : ApplicationStatus.StageTwoRejected;
        }

        private static bool IsNotRejected(Entities.Application application)
        {
            return application.Value.Status != ApplicationStatus.Rejected;
        }

        private static bool IsStageOneRejected(Entities.Application application)
        {
            return application.Value.StageTwo.Isae3000.Status == "CannotStartYet";
        }
    }
}