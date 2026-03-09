namespace lab30vN15
{
    public class DiscountCalculator
    {
        public decimal CalculateDiscount(decimal totalAmount, bool isLoyalCustomer, bool hasCoupon)
        {
            decimal discount = 0;

            if (totalAmount > 100)
                discount += totalAmount * 0.05m;

            if (isLoyalCustomer)
                discount += 5;

            if (hasCoupon)
                discount += 15;

            if (discount > 50)
                discount = 50;

            return discount;
        }

        public bool FreeShipping(decimal totalAmount)
        {
            return totalAmount >= 200;
        }
    }
}