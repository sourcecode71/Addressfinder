using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Addressfinder.Shared.Address
{
    public class IPAddressAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            UsaAddressViewModel usaAddress = (UsaAddressViewModel)validationContext.ObjectInstance;

            const string regexPattern = @"^([\d]{1,3}\.){3}[\d]{1,3}$";
            var regex = new Regex(regexPattern);
            if (string.IsNullOrEmpty(usaAddress.IP))
            {
                return new ValidationResult("IP address  is null");
            }
            if (!regex.IsMatch(usaAddress.IP) || usaAddress.IP.Split('.').SingleOrDefault(s => int.Parse(s) > 255) != null)
                return new ValidationResult("Invalid IP Address");


            return ValidationResult.Success;
        }
    }
}
