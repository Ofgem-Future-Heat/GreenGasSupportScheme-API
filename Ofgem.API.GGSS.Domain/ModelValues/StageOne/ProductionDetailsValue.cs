using System;
using Ofgem.API.GGSS.Domain.Models;

namespace Ofgem.API.GGSS.Domain.ModelValues.StageOne
{
    public class ProductionDetailsValue
    {
        public string Status { get; set; } = "NotStarted";
        public string MaximumInitialCapacity { get; set; }
        public string EligibleBiomethane { get; set; }
        public DateTime InjectionStartDate { get; set; }
    }
}