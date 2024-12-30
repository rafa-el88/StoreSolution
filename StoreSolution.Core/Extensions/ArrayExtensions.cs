namespace StoreSolution.Core.Extensions
{
    public static class ArrayExtensions
    {
        public static T[]? NullIfEmpty<T>(this T[]? value) => value?.Length == 0 ? null : value;

        public static T[]? EmptyIfNull<T>(this T[]? value) => value ?? [];
    }
}
