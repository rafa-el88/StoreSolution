using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace StoreSolution.Server.Atributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MinimumCountAttribute(int minCount, bool required = true, bool allowEmptyStringValues = false) :
        ValidationAttribute("'{0}' must have at least {1} item.")
    {
        public MinimumCountAttribute() : this(1) { }

        public override bool IsValid(object? value)
        {
            if (value == null)
                return !required;

            if (!allowEmptyStringValues && value is ICollection<string> stringList)
                return stringList.Count(s => !string.IsNullOrWhiteSpace(s)) >= minCount;

            if (value is ICollection list)
                return list.Count >= minCount;

            return false;
        }

        public override string FormatErrorMessage(string name) => string.Format(ErrorMessageString, name, minCount);
    }
}
