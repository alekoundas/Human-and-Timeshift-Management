using System.ComponentModel.DataAnnotations;

namespace DataAccess.DataAnnotation.Unique
{
    public class WorkPlaceValidateUnique : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var baseDbContext = (BaseDbContext)validationContext.GetService(typeof(BaseDbContext));
            var baseDataWork = new BaseDatawork(baseDbContext);
            if (baseDataWork.WorkPlaces.Any(x => x.Title == value.ToString().Trim()))
                return new ValidationResult("Ο τίτλος πρέπει να είναι μοναδικός");

            return ValidationResult.Success;
        }
    }
}
