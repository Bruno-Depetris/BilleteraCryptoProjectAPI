namespace BilleteraCryptoProjectAPI.Services
{
    public interface ICriptoyaService
    {
        Task<decimal> GetPriceAsync(string cryptoCode, string exchange = "satoshitango");
    }

    public class CriptoyaService : ICriptoyaService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://criptoya.com/api";

        public CriptoyaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<decimal> GetPriceAsync(string cryptoCode, string exchange = "satoshitango")
        {
            try
            {
                var url = $"{BaseUrl}/{exchange}/{cryptoCode.ToLower()}/ars/";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonDocument.Parse(json);

                if (result.RootElement.TryGetProperty("ask", out var askElement))
                {
                    if (decimal.TryParse(askElement.GetRawText(), out var price))
                    {
                        return price;
                    }
                }

                throw new Exception("No se pudo extraer el precio de la respuesta de la API");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el precio de {cryptoCode}: {ex.Message}", ex);
            }
        }
    }
}


