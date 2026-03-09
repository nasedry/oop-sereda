using Xunit;
using lab30vN15;

namespace lab30vN15.Tests
{
    public class DiscountCalculatorTests
    {
        private readonly DiscountCalculator calculator = new();

        [Fact]
        public void Test1_NoDiscount()
        {
            Assert.Equal(0, calculator.CalculateDiscount(50, false, false));
        }

        [Fact]
        public void Test2_LoyalCustomerOnly()
        {
            Assert.Equal(5, calculator.CalculateDiscount(50, true, false));
        }

        [Fact]
        public void Test3_HasCouponOnly()
        {
            Assert.Equal(15, calculator.CalculateDiscount(50, false, true));
        }

        [Fact]
        public void Test4_LoyalAndCoupon()
        {
            Assert.Equal(20, calculator.CalculateDiscount(50, true, true));
        }

        [Fact]
        public void Test5_AmountAbove100()
        {
            Assert.Equal(10, calculator.CalculateDiscount(200, false, false));
        }

        [Fact]
        public void Test6_MaxDiscountCapped()
        {
            Assert.Equal(50, calculator.CalculateDiscount(1000, true, true));
        }

        [Fact]
        public void Test7_LoyalAbove100()
        {
            Assert.Equal(15, calculator.CalculateDiscount(200, true, false));
        }

        [Fact]
        public void Test8_CouponAbove100()
        {
            Assert.Equal(25, calculator.CalculateDiscount(200, false, true));
        }

        [Fact]
        public void Test9_LoyalCouponAbove100()
        {
            Assert.Equal(30, calculator.CalculateDiscount(200, true, true));
        }

        [Fact]
        public void Test11_FreeShipping_True()
        {
            Assert.True(calculator.FreeShipping(250));
        }

        [Fact]
        public void Test12_FreeShipping_False()
        {
            Assert.False(calculator.FreeShipping(150));
        }
    }
}