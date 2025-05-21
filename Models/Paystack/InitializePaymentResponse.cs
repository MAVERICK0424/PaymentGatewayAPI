namespace PaymentGatewayAPI.Models.Paystack
{
    public class InitializePaymentResponse
    {
        public object Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
