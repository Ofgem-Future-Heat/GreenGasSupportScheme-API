using System.ComponentModel.DataAnnotations;

namespace Ofgem.API.GGSS.Domain.Models
{
    public class AddressModel
    {
        public string Name { get; set; }

        //[Required(ErrorMessage = "Enter building and street")]
        public string LineOne { get; set; }

        public string LineTwo { get; set; }

        //[Required(ErrorMessage = "Enter town or city")]
        public string Town { get; set; }

        public string County { get; set; }

        //[Required(ErrorMessage = "Enter postcode")]
        public string Postcode { get; set; }
    }
}
