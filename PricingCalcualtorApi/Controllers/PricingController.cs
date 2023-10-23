using Microsoft.AspNetCore.Mvc;

namespace PricingCalcualtorApi.Controllers
{
    [Route("api/pricing")]
    [ApiController]
    public class PricingController : ControllerBase
    {
        private readonly PricingService _pricingService;

        public PricingController(PricingService pricingService)
        {
            _pricingService = pricingService;
        }

        [HttpGet("calculate-price")]
        public IActionResult CalculatePrice(int customerId, DateTime startDate, DateTime endDate)
        {
            try
            {
                decimal totalPrice = _pricingService.CalculateTotalPrice(customerId, startDate, endDate);
                return Ok(new { TotalPrice = totalPrice });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
