namespace StoreSolution.Core.Services.Account.Exceptions
{
    public class UserNotFoundException : UserAccountException
    {
        public UserNotFoundException() : base("Unable to find the requested User.") { }

        public UserNotFoundException(string? message) : base(message) { }

        public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
