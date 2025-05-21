namespace PaymentGatewayAPI.Models.Paystack
{
    public class InitializePaymentRequest
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public decimal Amount { get; set; }
    }
}
