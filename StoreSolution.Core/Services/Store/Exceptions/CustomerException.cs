namespace StoreSolution.Core.Services.Store.Exceptions
{
    public class CustomerException : Exception
    {
        public CustomerException() : base("A Customer Exception has occurred.")
        {
        }

        public CustomerException(string? message) : base(message)
        {
        }

        public CustomerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
