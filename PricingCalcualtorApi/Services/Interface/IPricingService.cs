using PricingCalcualtorApi.Models;

namespace PricingCalcualtorApi.Services.Interface
{
    public interface IPricingService
    {
        decimal CalculateTotalPrice(int customerId, DateTime startDate, DateTime endDate);
    }
}