namespace PaymentGatewayAPI.Models.Paystack
{
    public class InitializePaymentRequest
    {
        public int Amount { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Currency { get; set; } = "NGN";
    }
}
