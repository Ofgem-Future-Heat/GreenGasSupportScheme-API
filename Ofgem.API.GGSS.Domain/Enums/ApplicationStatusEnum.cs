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
        OnHold,
        [Display(Name = "With Applicant")]
        WithApplicant,
        [Display(Name = "Stage One In Review")]
        StageOneInReview,
        [Display(Name = "Stage Two In Review")]
        StageTwoInReview,
        [Display(Name = "Stage Three In Review")]
        StageThreeInReview,
        [Display(Name = "Stage One Rejected")]
        StageOneRejected,
        [Display(Name = "Stage Two Rejected")]
        StageTwoRejected,
        [Display(Name = "Stage Three Rejected")]
        StageThreeRejected,
        [Display(Name = "Stage One With Applicant")]
        StageOneWithApplicant,
        [Display(Name = "Stage Two With Applicant")]
        StageTwoWithApplicant,
        [Display(Name = "Stage Three With Applicant")]
        StageThreeWithApplicant,
    }
}
