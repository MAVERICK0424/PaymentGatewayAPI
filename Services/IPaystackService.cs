using PaymentGatewayAPI.Models;
using PaymentGatewayAPI.Models.Paystack;
using System.Threading.Tasks;

namespace PaymentGatewayAPI.Services
{
    public interface IPaystackService
    {
        Task<PaymentResponse?> InitializePaymentAsync(InitializePaymentRequest request);
        Task<PaymentResponse> ProcessPaymentAsync(PaymentDto payment);
        Task<PaymentResponse> VerifyPaymentAsync(string reference);
    }
}
