using System.ComponentModel.DataAnnotations;

namespace DataAccess.DataAnnotation.Unique
{
    public class EmployeeValidateUnique : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var baseDbContext = (BaseDbContext)validationContext.GetService(typeof(BaseDbContext));
            var baseDataWork = new BaseDatawork(baseDbContext);
            if (baseDataWork.Employees.Any(x => x.Afm == value.ToString().Trim()))
                return new ValidationResult("Το ΑΦΜ πρέπει να είναι μοναδικό");

            return ValidationResult.Success;
        }
    }
}
