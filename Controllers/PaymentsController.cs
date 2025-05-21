using Microsoft.AspNetCore.Mvc;
using PaymentGatewayAPI.Models;
using PaymentGatewayAPI.Models.Paystack;
using PaymentGatewayAPI.Services;
using System.Threading.Tasks;

namespace PaymentGatewayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaystackService _paystackService;

        public PaymentsController(IPaystackService paystackService)
        {
            _paystackService = paystackService;
        }

        [HttpPost("initialize")]
        public async Task<IActionResult> InitializePayment([FromBody] InitializePaymentRequest request)
        {
            var response = await _paystackService.InitializePaymentAsync(request);

            if (response == null)
                return StatusCode(500, "Failed to initialize payment");

            return Ok(response);
        }

        // Added route "process" for clarity and testing
        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDto paymentDto)
        {
            var response = await _paystackService.ProcessPaymentAsync(paymentDto);

            if (response == null)
                return NotFound("Payment not found or failed");

            return Ok(response);
        }
    }
}
