namespace PricingCalcualtorApi.Models
{
    public class Price
    {
        public int PriceId { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public decimal BasePrice { get; set; }
        public int Discount { get; set; } = 0;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
    }

}
