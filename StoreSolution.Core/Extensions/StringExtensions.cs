namespace StoreSolution.Core.Extensions
{
    public static class StringExtensions
    {
        public static string? NullIfWhiteSpace(this string? value) => string.IsNullOrWhiteSpace(value) ? null : value;
    }
}
