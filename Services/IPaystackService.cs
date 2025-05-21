using PaymentGatewayAPI.Models;
using PaymentGatewayAPI.Models.Paystack;
using System.Threading.Tasks;

namespace PaymentGatewayAPI.Services
{
    public interface IPaystackService
    {
        Task<InitializePaymentResponse> InitializePaymentAsync(InitializePaymentRequest request);
        Task<PaymentResponse> ProcessPaymentAsync(PaymentDto payment);
    }
}
