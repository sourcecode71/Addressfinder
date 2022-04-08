using Addressfinder.Repository.Address;
using Addressfinder.Services.Address;
using Addressfinder.Shared.Address;
using Addressfinder.Shared.Auth;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using static Addressfinder.Shared.Address.StateList;

namespace Addressfinder.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly IRegionRepository _regionRepository;
        public AddressController(IConfiguration configuration, IRegionRepository regionRepository)
        {
            _configuration = configuration;
            _regionRepository = regionRepository;
        }

        /// <summary>
        /// Get Address by IP.
        /// IP info provide the address.
        /// </summary>
        /// <param name="IP"> IP Address</param>
        /// <returns>Full Adress will return</returns>
        
        [HttpGet]
        public async Task<UsaAddressViewModel> GetAddressByIP(string IP)
        {
            try 
            {
                var baseUrl = _configuration.GetSection("IpInfo").GetSection("baseUrl").Value;
                var token = _configuration.GetSection("IpInfo").GetSection("token").Value;
                string URL =String.Format("{0}/{1}/?token={2}",baseUrl,IP,token);

                IPToFullAddress iPToFullAddress = new IPToFullAddress {Url = URL};

                var fullAddressAdapter = new IPInfoToFullAddressAdapter(iPToFullAddress);
                string fullAddress = await fullAddressAdapter.GetFullAddressByIP();
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                UsaAddressViewModel viewModel = serializer.Deserialize<UsaAddressViewModel>(fullAddress);

                return viewModel;

            }
            catch (Exception ex)
            { 

                throw ex;
            }
        }

        /// <summary>
        ///  Get All region/state of USA.
        /// </summary>
        /// <returns>All states of the USA </returns>

        [HttpGet("region")]
        public IEnumerable<State> GetRegions()
        {
            return _regionRepository.ListOfStates();
        }

        /// <summary>
        ///  Save address
        /// </summary>

        [HttpPost("my-address")]
        public async Task Save([FromBody] GuestUserViewModel viewModel)
        {
            var userMod = viewModel;
            // TODO : Save or Other operation 
        }



    }
}
