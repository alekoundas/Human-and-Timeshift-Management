using System.ComponentModel.DataAnnotations;

namespace DataAccess.DataAnnotation.Unique
{
    public class LeaveTypeValidateUnique : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var baseDbContext = (BaseDbContext)validationContext.GetService(typeof(BaseDbContext));
            var baseDataWork = new BaseDatawork(baseDbContext);
            if (baseDataWork.LeaveTypes.Any(x => x.Name == value.ToString().Trim()))
                return new ValidationResult("Το όνομα πρέπει να είναι μοναδικό");

            return ValidationResult.Success;
        }
    }
}
