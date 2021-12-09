using MediatR;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.Responses.Applications;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ofgem.API.GGSS.Domain.Commands.Applications
{
    public class StageOne : IRequest<ApplicationResponse>
    {
        [Required]
        public Guid OrganisationId { get; set; }
        public string InstallationName { get; set; }
        public decimal MaxCapacity { get; set; }
        public DateTime DateInjectionStart { get; set; }
        public string Location { get; set; }
        public AddressModel InstallationAddress { get; set; }

        #region Documents

        public DocumentValue CapacityCheckDocument { get; set; }

        public DocumentValue PlanningPermissionDocument { get; set; }

        #endregion Documents
    }
}
