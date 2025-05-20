namespace PaymentGatewayAPI.Models
{
    public class PaymentRequest
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
