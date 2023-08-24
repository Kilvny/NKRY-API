namespace NKRY_API.Helpers
{
    public static class DateTimeExtensions
    {
        public static int CalculateCurrentAge(this DateTime dateTime)
        {
            var currentDate = DateTime.UtcNow;
            int age = currentDate.Day - dateTime.Day;
            return age;
        }
    }
}
