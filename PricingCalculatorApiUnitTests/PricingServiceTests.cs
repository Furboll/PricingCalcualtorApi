using Microsoft.EntityFrameworkCore;
using PricingCalcualtorApi.Data;
using PricingCalcualtorApi.Models;
using PricingCalcualtorApi.Models.Enums;

namespace PricingCalculatorApiUnitTests
{
    public class PricingServiceTests
    {

        [Fact]
        public void CalculateTotalPrice_CustomerX_Test_Case_One()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PricingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new PricingDbContext(options))
            {
                context.Database.EnsureDeleted(); 
                context.Database.EnsureCreated();

                var customer = new Customer
                {
                    CustomerId = 1,
                    Name = "CustomerX"
                };

                var priceA = new Price
                {
                    PriceId = 1,
                    CustomerId = 1,
                    ServiceId = 1, // Service A
                    BasePrice = 0.2m,
                    StartDate = new DateTime(2019, 9, 20)
                };

                var priceC = new Price
                {
                    PriceId = 2,
                    CustomerId = 1,
                    ServiceId = 3, // Service C
                    BasePrice = 0.4m,
                    Discount = 20, // 20% discount
                    StartDate = new DateTime(2019, 9, 20),
                    DiscountStartDate = new DateTime(2019, 9, 22),
                    DiscountEndDate = new DateTime(2019, 9, 24)
                };

                var serviceA = new Service
                {
                    Name = "Service A",
                    ServiceId = 1,
                    Prices = new List<Price> { priceA },
                    ServiceType = ServiceType.WorkingDaysOnly
                };

                var serviceC = new Service
                {
                    Name = "Service C",
                    ServiceId = 3,
                    Prices = new List<Price> { priceC },
                    ServiceType = ServiceType.Alldays
                };

                context.Customers.Add(customer);
                context.Prices.Add(priceA);
                context.Prices.Add(priceC);
                context.Services.AddRange(serviceA, serviceC);
                context.SaveChanges();
            }

            var pricingService = new PricingService(new PricingDbContext(options));

            // Act
            var totalPrice = pricingService.CalculateTotalPrice(1, new DateTime(2019, 9, 20), new DateTime(2019, 10, 01));

            // Assert
            Assert.Equal(6.16m, totalPrice);
        }

        [Fact]
        public void CalculateTotalPrice_CustomerY_Test_Case_Two()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PricingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new PricingDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var customer = new Customer
                {
                    CustomerId = 2,
                    Name = "CustomerY",
                    FreeDays = 200
                };

                var priceB = new Price
                {
                    PriceId = 2,
                    CustomerId = 2,
                    ServiceId = 2, // Service B
                    BasePrice = 0.24m,
                    Discount = 30,
                    StartDate = new DateTime(2018, 01, 01),
                    DiscountStartDate = new DateTime(2018, 07, 20),
                    DiscountEndDate = new DateTime(2019, 10, 1)
                };

                var priceC = new Price
                {
                    PriceId = 3,
                    CustomerId = 2,
                    ServiceId = 3, // Service C
                    BasePrice = 0.4m,
                    Discount = 30,
                    StartDate = new DateTime(2018, 01, 01),
                    DiscountStartDate = new DateTime(2018, 07, 20),
                    DiscountEndDate = new DateTime(2019, 10, 1)
                };

                var serviceB = new Service
                {
                    Name = "Service B",
                    ServiceId = 2,
                    Prices = new List<Price> { priceB },
                    ServiceType = ServiceType.WorkingDaysOnly
                };

                var serviceC = new Service
                {
                    Name = "Service C",
                    ServiceId = 3,
                    Prices = new List<Price> { priceC },
                    ServiceType = ServiceType.WorkingDaysOnly
                };

                context.Customers.Add(customer);
                context.Prices.AddRange(priceB, priceC);
                context.Services.AddRange(serviceB, serviceC);
                context.SaveChanges();
            }

            var pricingService = new PricingService(new PricingDbContext(options));

            // Act
            var totalPrice = pricingService.CalculateTotalPrice(2, new DateTime(2018, 01, 01), new DateTime(2019, 10, 1));

            // Assert
            Assert.Equal(104.384m, totalPrice);
        }

    }
}