using System.ComponentModel.DataAnnotations;

namespace DataAccess.DataAnnotation.Unique
{
    public class ContractValidateUnique : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var baseDbContext = (BaseDbContext)validationContext.GetService(typeof(BaseDbContext));
            var baseDataWork = new BaseDatawork(baseDbContext);
            if (baseDataWork.Contracts.Any(x => x.Title == value.ToString().Trim()))
                return new ValidationResult("Ο Τίτλος πρέπει να είναι μοναδικός");

            return ValidationResult.Success;
        }
    }
}
