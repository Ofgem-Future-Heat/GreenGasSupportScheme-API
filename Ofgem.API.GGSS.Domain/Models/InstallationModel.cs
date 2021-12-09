using Ofgem.API.GGSS.Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Ofgem.API.GGSS.Domain.Models
{
    public class InstallationModel
    {
        public string Name { get; set; }
        [Required]
        [ValidLocation(ErrorMessage = "Must be England, Scotland or Wales only.")]
        public string Location { get; set; }
        public AddressModel Address { get; set; }
    }
}
