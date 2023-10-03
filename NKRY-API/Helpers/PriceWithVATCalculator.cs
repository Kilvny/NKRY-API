namespace NKRY_API.Helpers
{
    public static class PriceWithVATCalculator
    {
        public static decimal Calculate(this decimal price, int VATRate)
        {
            return price * (VATRate / 100);
        }
    }
}
