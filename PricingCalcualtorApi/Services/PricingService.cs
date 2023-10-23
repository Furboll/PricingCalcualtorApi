using Microsoft.EntityFrameworkCore;
using PricingCalcualtorApi.Data;
using PricingCalcualtorApi.Models.Enums;
using PricingCalcualtorApi.Services.Interface;

public class PricingService : IPricingService
{
    private readonly PricingDbContext _context;

    public PricingService(PricingDbContext context)
    {
        _context = context;
    }
    public decimal CalculateTotalPrice(int customerId, DateTime startDate, DateTime endDate)
    {
        var customer = _context.Customers
            .Include(c => c.Prices)
            .ThenInclude(p => p.Service)
            .SingleOrDefault(c => c.CustomerId == customerId);

        if (customer == null)
        {
            throw new ArgumentException("Customer not found.");
        }

        var totalDaysInSpan = (int)(endDate - startDate).TotalDays + 1;

        if (totalDaysInSpan <= customer.FreeDays) 
        {
            return 0;
        }

        var totalPrice = 0.0m;

        foreach (var price in customer.Prices)
        {
            var partPrice = 0.0m;
            var totalDaysToCalculate = CalculateDays(startDate, endDate, price.StartDate, price.Service.ServiceType);
            var totalDiscountDays = 0;

            if (price.DiscountStartDate.Year > 1900)
            { 
                totalDiscountDays += CalculateDiscountDays(price.DiscountStartDate, price.DiscountEndDate, price.Service.ServiceType); 
            }

            if (customer.FreeDays > 0)
            {
                totalDaysToCalculate -= customer.FreeDays;
            }

            var fullPrice = (totalDaysToCalculate -= totalDiscountDays) * price.BasePrice;
            var discountPrice = price.BasePrice * (100 - price.Discount) / 100 * totalDiscountDays;

            partPrice = fullPrice + discountPrice;
         
            totalPrice += partPrice;
        }

        return totalPrice;
    }

    static int CalculateDiscountDays(DateTime discountStartDate, DateTime discountEndDate, ServiceType type)
    {
        int days = 0;
        if (type == ServiceType.WorkingDaysOnly) 
        {
            while (discountStartDate <= discountEndDate) 
            {
                if (discountStartDate.DayOfWeek != DayOfWeek.Saturday && discountStartDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    days++;
                }
                discountStartDate = discountStartDate.AddDays(1);
            }
        }
        else if (type == ServiceType.Alldays)
        {
            TimeSpan difference = discountEndDate - discountStartDate;
            days = difference.Days + 1;
        }
        return days;
    }

    static int CalculateDays(DateTime startDate, DateTime endDate, DateTime serviceStartDate, ServiceType type)
    {
        int days = 0;

        if (type == ServiceType.WorkingDaysOnly)
        {
            while (serviceStartDate <= endDate)
            {
                if (serviceStartDate >= startDate && serviceStartDate.DayOfWeek != DayOfWeek.Saturday && serviceStartDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    days++;
                }
                serviceStartDate = serviceStartDate.AddDays(1);
            }
        }
        else if (type == ServiceType.Alldays)
        {
            TimeSpan difference = endDate - serviceStartDate;
            days = difference.Days + 1;
        }

        return days;
    }
}
