using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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

        public async Task<PaymentResponse?> InitializePaymentAsync(InitializePaymentRequest request)
        {
            var paystackKey = _configuration["Paystack:SecretKey"];

            var payload = new
            {
                email = request.CustomerEmail,
                amount = (int)(request.Amount * 100) // Paystack expects amount in kobo
            };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", paystackKey);

            var response = await _httpClient.PostAsync("https://api.paystack.co/transaction/initialize", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new PaymentResponse
                {
                    Status = "failed",
                    CustomerName = request.CustomerName,
                    CustomerEmail = request.CustomerEmail,
                    Amount = request.Amount,
                    Message = "Payment initialization failed."
                };
            }

            // Deserialize Paystack response
            dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseString);
            string reference = jsonResponse.data.reference;
            string authUrl = jsonResponse.data.authorization_url;

            return new PaymentResponse
            {
                Id = reference,
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                Amount = request.Amount,
                Status = "pending",
                AuthorizationUrl = authUrl,
                Message = "Payment initiated. Redirect user to authorization_url."
            };
        }

        public async Task<PaymentResponse> VerifyPaymentAsync(string reference)
        {
            var paystackKey = _configuration["Paystack:SecretKey"];

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", paystackKey);

            var response = await _httpClient.GetAsync($"https://api.paystack.co/transaction/verify/{reference}");
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new PaymentResponse
                {
                    Id = reference,
                    Status = "failed",
                    Message = "Failed to verify payment."
                };
            }

            dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseString);
            var status = jsonResponse.data.status.ToString();

            return new PaymentResponse
            {
                Id = reference,

                CustomerEmail = jsonResponse.data.customer.email ?? "Unknown",
                Amount = ((decimal)jsonResponse.data.amount) / 100, // convert from kobo to naira
                Status = status == "success" ? "completed" : "failed",
                Message = "Payment verification completed."
            };
        }


        public async Task<PaymentResponse> ProcessPaymentAsync(PaymentDto payment)
        {
            var requestData = new
            {
                amount = payment.Amount,
                email = payment.Email,
                currency = payment.Currency
            };

            var jsonContent = System.Text.Json.JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/transaction/initialize", content);

            if (!response.IsSuccessStatusCode)
            {
                return new PaymentResponse
                {
                    Status = "failed",
                    Message = "Payment initialization failed."
                };
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var paymentResponse = System.Text.Json.JsonSerializer.Deserialize<PaymentResponse>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return paymentResponse ?? new PaymentResponse
            {
                Status = "failed",
                Message = "Failed to parse payment response."
            };
        }
    }
}
