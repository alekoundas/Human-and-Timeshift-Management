using DataAccess.Models.Entity;
using LinqKit;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAccess.DataAnnotation
{
    public class WorkPlaceHourRestrictionValidateUnique : ValidationAttribute
    {
        private readonly string _currentPropertyName;
        public WorkPlaceHourRestrictionValidateUnique(string currentPropertyName)
        {
            _currentPropertyName = currentPropertyName;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {

            var baseDbContext = (BaseDbContext)validationContext.GetService(typeof(BaseDbContext));
            var baseDataWork = new BaseDatawork(baseDbContext);
            var filter = PredicateBuilder.New<WorkPlaceHourRestriction>();

            var workPlaceIdProperty = validationContext.ObjectType.GetProperty("WorkPlaceId");
            var monthProperty = validationContext.ObjectType.GetProperty("Month");
            var yearProperty = validationContext.ObjectType.GetProperty("Year");

            var workPlaceIdValue = workPlaceIdProperty.GetValue(validationContext.ObjectInstance);
            var monthValue = monthProperty.GetValue(validationContext.ObjectInstance);
            var yearValue = yearProperty.GetValue(validationContext.ObjectInstance);

            filter = filter.And(x => x.WorkPlaceId == (int)workPlaceIdValue);
            filter = filter.And(x => x.Year == (int)yearValue);
            filter = filter.And(x => x.Month == (int)monthValue);

            if (baseDataWork.WorkPlaceHourRestrictions.Any(filter))
            {
                var possibleFields = new Dictionary<string, string>
                    {
                        { "Month", "Μήνας" },
                        { "Year", "Έτος" },
                        { "WorkPlaceId", "Πόστο" }
                    };
                possibleFields.Remove(_currentPropertyName, out var first);
                possibleFields.Remove(possibleFields.ElementAt(0).Key, out var second);
                possibleFields.Remove(possibleFields.ElementAt(0).Key, out var third);
                return new ValidationResult($"Το πεδίο {first} πρέπει να εχει μοναδικό σε συνδυασμό με το {second} και το {third}");
            }

            return ValidationResult.Success;
        }
    }
}
