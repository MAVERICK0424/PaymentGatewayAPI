using Microsoft.AspNetCore.Mvc;
using PaymentGatewayAPI.Models;
using PaymentGatewayAPI.Services;

namespace PaymentGatewayAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PaymentsController : ControllerBase
    {

        //private static readonly List<Payment> Payments = new();
        private readonly IPaystackService _paystackService;

        public PaymentsController(IPaystackService paystackService)
        {
            _paystackService = paystackService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Payment request)
        {
            var paymentId = Guid.NewGuid().ToString();

            var result = new
            {
                payment = new
                {
                    id = paymentId,
                    customer_name = request.CustomerName,
                    customer_email = request.CustomerEmail,
                    amount = request.Amount,
                    status = "completed"
                },
                status = "success",
                message = "Payment initiated successfully."
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(new
            {
                payment = new
                {
                    Id = id,
                    customer_name = "John Doe",
                    customer_email = "john@example.com",
                    amount = 50.00,
                    status = "completed"
                },
                status = "success",
                message = "Payment details retrieved successfully."
            });
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDto paymentDto)
        {
            var result = await _paystackService.ProcessPaymentAsync(paymentDto);
            return Ok(result);
        }

    }
}
