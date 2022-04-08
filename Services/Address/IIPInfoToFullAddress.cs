namespace Addressfinder.Services.Address
{
    public interface IIPInfoToFullAddress
    {
       Task<string> GetFullAddressByIP();
    }
}
