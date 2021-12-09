using FluentValidation;
using Ofgem.API.GGSS.Domain.Commands.Applications;

namespace Ofgem.API.GGSS.Application.Validators
{
    public class StageThreeSaveValidator : AbstractValidator<ApplicationSave<StageThree>>
    {
        public StageThreeSaveValidator()
        {

        }
    }
}
