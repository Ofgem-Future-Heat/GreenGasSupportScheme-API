using System.ComponentModel.DataAnnotations;

namespace Ofgem.API.GGSS.Domain.Enums
{
    public enum ApplicationStatus
    {
        Draft,
        [Display(Name = "Stage One Submitted")]
        StageOneSubmitted,
        [Display(Name = "Stage Two Submitted")]
        StageTwoSubmitted,
        [Display(Name = "Stage Three Submitted")]
        StageThreeSubmitted,
        [Display(Name = "Stage One Approved")]
        StageOneApproved,
        [Display(Name = "Stage Two Approved")]
        StageTwoApproved,
        [Display(Name = "Stage Three Approved")]
        StageThreeApproved,
        Rejected,
        OnHold
    }
}
