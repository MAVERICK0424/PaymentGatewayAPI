using PaymentGatewayAPI.Models;

namespace PaymentGatewayAPI.Services
{
    public class PaystackService : IPaystackService
    {
        public async Task<PaymentResponse> ProcessPaymentAsync(PaymentDto paymentDto)
        {
            await Task.Delay(100); 

            return new PaymentResponse
            {
                Status = true,
                Message = "Payment processed successfully."
            };
        }
    }
}
