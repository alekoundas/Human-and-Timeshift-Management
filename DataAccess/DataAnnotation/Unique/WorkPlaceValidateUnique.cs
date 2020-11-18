using DataAccess.Models.Entity;
using LinqKit;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAccess.DataAnnotation.Unique
{
    public class WorkPlaceValidateUnique : ValidationAttribute
    {
        private readonly string _currentPropertyName;
        public WorkPlaceValidateUnique(string currentPropertyName)
        {
            _currentPropertyName = currentPropertyName;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {

            var baseDbContext = (BaseDbContext)validationContext.GetService(typeof(BaseDbContext));
            var baseDataWork = new BaseDatawork(baseDbContext);
            var filter = PredicateBuilder.New<WorkPlace>();

            var customerIdProperty = validationContext.ObjectType.GetProperty("CustomerId");
            var titleProperty = validationContext.ObjectType.GetProperty("Title");

            var customerIdValue = customerIdProperty.GetValue(validationContext.ObjectInstance);
            var titleValue = titleProperty.GetValue(validationContext.ObjectInstance);
            if ((int?)customerIdValue != null)
                filter = filter.And(x => x.CustomerId == (int?)customerIdValue);
            filter = filter.And(x => x.Title == titleValue);

            if (baseDataWork.WorkPlaces.Any(filter))
            {
                var possibleFields = new Dictionary<string, string>
                    {
                        { "CustomerId", "Πελάτης" },
                        { "Title", "Τίτλος" },
                    };

                possibleFields.Remove(_currentPropertyName, out var first);
                possibleFields.Remove(possibleFields.ElementAt(0).Key, out var second);
                return new ValidationResult($"Το πεδίο {first} πρέπει να εχει μοναδικό σε συνδυασμό με το {second} ");
            }

            return ValidationResult.Success;
        }
    }
}
