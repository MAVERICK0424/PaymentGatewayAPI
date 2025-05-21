namespace PaymentGatewayAPI.Models
{
    public class PaymentResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
