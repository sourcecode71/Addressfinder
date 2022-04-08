namespace Addressfinder.Services.Address
{
    public class IPToFullAddress
    {
        public string Url { get; set; } 
        public async Task<string> GetFullAddress()
        {
            try
            {
                var client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(Url);
                HttpContent responseContent = response.Content;

                using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                {
                    string fullAddressResponse = await reader.ReadToEndAsync();
                    return fullAddressResponse;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
