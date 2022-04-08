using Addressfinder.Shared.Address;
using System.ComponentModel.DataAnnotations;

namespace Addressfinder.Shared.Auth
{
    public class GuestUserViewModel
    {
        [Required(ErrorMessage = "Fist name is required")]
        [StringLength(15, ErrorMessage = "First name is too long")]
        public string FirstName { get; set; }

       
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(15, ErrorMessage = "Last name is too long")]
        public string LastName { get; set; }
        public UsaAddressViewModel UaVM { get; set; }
    }
}
