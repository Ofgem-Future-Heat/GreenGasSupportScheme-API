using Ofgem.API.GGSS.Domain.Models;
using System;
using Ofgem.API.GGSS.Domain.ModelValues.StageOne;
using Ofgem.API.GGSS.Domain.ModelValues.StageTwo;
using Ofgem.API.GGSS.Domain.ModelValues.StageThree;

namespace Ofgem.API.GGSS.Domain.ModelValues
{
    public class ApplicationValue
    {
        public string InstallationName { get; set; }
        public Enums.Location Location { get; set; }
        public AddressModel InstallationAddress { get; set; }
        public decimal MaxCapacity { get; set; }
        public DateTime DateInjectionStart { get; set; }
        public Enums.ApplicationStatus Status { get; set; }
        public string Reference { get; set; }
        public string LastModified { get; set; }
        public StageOneValue StageOne { get; set; } = new StageOneValue();
        public StageTwoValue StageTwo { get; set; } = new StageTwoValue();
        public StageThreeValue StageThree { get; set; } = new StageThreeValue();
        public bool CanSubmit { get; set; } = false;
    }
}
