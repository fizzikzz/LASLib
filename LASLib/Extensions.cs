namespace LASLib
{
    public static class Extensions
    {
        public static bool IsMd(this string value)
        {
            return
                value.Equals("DEPT", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("DEPTH", StringComparison.OrdinalIgnoreCase);
        }
        public static bool IsTvd(this string value)
        {
            return
                value.Equals("TVD", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("TVD DEPTH", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("TVD_DEPTH", StringComparison.OrdinalIgnoreCase);
        }
        public static bool IsDate(this string value)
        {
            return
                value.Equals("DATE", StringComparison.OrdinalIgnoreCase);
        }
        public static bool IsTime(this string value)
        {
            return
                value.Equals("TIME", StringComparison.OrdinalIgnoreCase);
        }
    }
}
