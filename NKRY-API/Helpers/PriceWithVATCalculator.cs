namespace NKRY_API.Helpers
{
    public static class PriceWithVATCalculator
    {

        public static decimal CalculateOnlyVAT(decimal price, int VATRate)
        {
            return price * (0.15M);
        }
        public static decimal Calculate(this decimal price, int VATRate)
        {
            return CalculateOnlyVAT(price, VATRate) + price;
        }
    }
}
