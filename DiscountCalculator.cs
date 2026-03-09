using System;

namespace lab30vN15
{
    public class DiscountCalculator
    {
        public decimal CalculateDiscount(decimal price, decimal discountPercent)
        {
            if (price < 0) throw new ArgumentException("Price cannot be negative");
            if (discountPercent < 0 || discountPercent > 100)
                throw new ArgumentException("Discount percent must be between 0 and 100");

            return price * discountPercent / 100;
        }

        public decimal ApplyCoupon(decimal price, decimal couponValue)
        {
            if (price < 0) throw new ArgumentException("Price cannot be negative");
            if (couponValue < 0) throw new ArgumentException("Coupon value cannot be negative");

            decimal finalPrice = price - couponValue;
            return finalPrice < 0 ? 0 : finalPrice;
        }
    }
}