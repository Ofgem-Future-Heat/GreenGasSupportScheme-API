using FluentValidation;
using Ofgem.API.GGSS.Domain.Commands.Applications;

namespace Ofgem.API.GGSS.Application.Validators
{
    public class StageOneSaveValidator : AbstractValidator<ApplicationSave<StageOne>>
    {
        public StageOneSaveValidator()
        {
        }
    }
}
