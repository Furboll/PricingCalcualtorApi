namespace PricingCalcualtorApi.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public List<Price> Prices { get; set; }
        public int FreeDays { get; set; }
    }
}
