using Microsoft.Extensions.Configuration;
using PaymentGatewayAPI.Models;
using PaymentGatewayAPI.Models.Paystack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentGatewayAPI.Services
{
    public class PaystackService : IPaystackService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PaystackService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["Paystack:BaseUrl"]!)
            };

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _configuration["Paystack:SecretKey"]);
        }

        public async Task<InitializePaymentResponse?> InitializePaymentAsync(InitializePaymentRequest request)
        {
            var jsonContent = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/transaction/initialize", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<InitializePaymentResponse>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<PaymentResponse> ProcessPaymentAsync(PaymentDto payment)
        {
            var requestData = new
            {
                amount = payment.Amount,
                email = payment.Email,
                currency = payment.Currency
            };

            var jsonContent = JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/transaction/initialize", content);

            if (!response.IsSuccessStatusCode)
            {
                return new PaymentResponse
                {
                    Status = false,
                    Message = "Payment initialization failed."
                };
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonSerializer.Deserialize<PaymentResponse>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return paymentResponse ?? new PaymentResponse
            {
                Status = false,
                Message = "Failed to parse payment response."
            };
        }
    }
}
