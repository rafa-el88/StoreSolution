namespace StoreSolution.Core.Services.Account.Exceptions
{
    public class UserRoleException : Exception
    {
        public UserRoleException() : base("A User Role Exception has occurred.") { }

        public UserRoleException(string? message) : base(message) { }

        public UserRoleException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
