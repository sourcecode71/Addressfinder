using System.ComponentModel.DataAnnotations;

namespace Addressfinder.Shared.Address
{
    public class UsaAddressViewModel
    {
        [Required(ErrorMessage = "IP is required")]
        [IPAddressAttribute]
        [StringLength(15)]
        public string IP { get; set; }
       
        [MaxLength(150)]
        [Required(ErrorMessage = "State address is required")]
        public string SateAddress { get; set; }
       
        [MaxLength(100)]
        public string? AptSuite { get; set; }
      
        [MaxLength(100)]
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
       
        [MaxLength(100)]
        [Required(ErrorMessage = "Region is required")]
        public string Region { get; set; }

        [MaxLength(10)]
        [Required(ErrorMessage = "Zip code is required")]
        public string postal { get; set; } 

    }
}
