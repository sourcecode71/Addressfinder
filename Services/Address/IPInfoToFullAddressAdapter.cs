using System.Web;

namespace Addressfinder.Services.Address
{
    public class IPInfoToFullAddressAdapter : IIPInfoToFullAddress
    {
        private readonly IPToFullAddress _iPToFullAddress;

        public IPInfoToFullAddressAdapter(IPToFullAddress iPToFullAddress)
        {
            _iPToFullAddress = iPToFullAddress;
        }
        public async Task<string> GetFullAddressByIP()
        {
            string fullAddress = await _iPToFullAddress.GetFullAddress();
            return fullAddress;

        }
    }
}
