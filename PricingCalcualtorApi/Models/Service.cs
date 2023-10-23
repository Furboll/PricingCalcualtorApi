using PricingCalcualtorApi.Models.Enums;

namespace PricingCalcualtorApi.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public List<Price> Prices { get; set; }
        public ServiceType ServiceType { get; set; }
    } 
}
