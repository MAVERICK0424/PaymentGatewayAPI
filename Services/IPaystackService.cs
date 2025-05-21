using PaymentGatewayAPI.Models;

namespace PaymentGatewayAPI.Services
{
    public interface IPaystackService
    {
        Task<PaymentResponse> ProcessPaymentAsync(PaymentDto paymentDto);
    }
}
