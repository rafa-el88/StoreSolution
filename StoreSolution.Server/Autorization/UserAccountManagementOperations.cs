using StoreSolution.Server.Autorization.Requeriments;

namespace StoreSolution.Server.Autorization
{
    public static class UserAccountManagementOperations
    {
        public const string CreateOperationName = "Create";
        public const string ReadOperationName = "Read";
        public const string UpdateOperationName = "Update";
        public const string DeleteOperationName = "Delete";

        public static readonly UserAccountAuthorizationRequirement CreateOperationRequirement = new(CreateOperationName);
        public static readonly UserAccountAuthorizationRequirement ReadOperationRequirement = new(ReadOperationName);
        public static readonly UserAccountAuthorizationRequirement UpdateOperationRequirement = new(UpdateOperationName);
        public static readonly UserAccountAuthorizationRequirement DeleteOperationRequirement = new(DeleteOperationName);
    }
}
