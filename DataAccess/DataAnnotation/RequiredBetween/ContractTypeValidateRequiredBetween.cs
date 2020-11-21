using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.DataAnnotation.RequiredBetween
{

    public class ContractTypeValidateRequiredBetween : ValidationAttribute
    {
        private readonly string _currentPropertyName;
        public ContractTypeValidateRequiredBetween(string currentPropertyName)
        {
            _currentPropertyName = currentPropertyName;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {

            var nameProperty = validationContext.ObjectType.GetProperty("Name");
            var startOnProperty = validationContext.ObjectType.GetProperty("StartOn");
            var endOnProperty = validationContext.ObjectType.GetProperty("EndOn");

            var nameValue = nameProperty.GetValue(validationContext.ObjectInstance);
            var startOnValue = startOnProperty.GetValue(validationContext.ObjectInstance);
            var endOnValue = endOnProperty.GetValue(validationContext.ObjectInstance);

            if (startOnValue == null || endOnValue == null)
            {
                if (nameValue == null)
                {
                    var possibleFields = new Dictionary<string, string>
                    {
                        { "Name", "Όνομα" },
                        { "StartOn", "Έναρξη" },
                        { "EndOn", "Λήξη" },
                    };

                    possibleFields.Remove(_currentPropertyName, out var first);
                    return new ValidationResult($"Το πεδίο {first} πρέπει να συμπληρωθεί ");
                }
            }

            return ValidationResult.Success;
        }
    }
}
