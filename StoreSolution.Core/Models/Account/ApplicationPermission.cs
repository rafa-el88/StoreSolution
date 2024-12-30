using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace StoreSolution.Core.Models.Account
{
    public class ApplicationPermission(string name, string value, string groupName, string? description = null)
    {
        //[JsonProperty("name")]
        public string Name { get; set; } = name;

        //[JsonProperty("value")]
        public string Value { get; set; } = value;

        //[JsonProperty("groupName")]
        public string GroupName { get; set; } = groupName;

        //[JsonProperty("description")]
        public string? Description { get; set; } = description;

        public override string ToString() => Value;

        [return: NotNullIfNotNull(nameof(permission))]
        public static implicit operator string?(ApplicationPermission? permission)
        {
            return permission?.Value;
        }
    }
}
