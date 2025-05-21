namespace PaymentGatewayAPI.Models
{
    public class Payment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Status { get; set; } = "pending";
    }
}
