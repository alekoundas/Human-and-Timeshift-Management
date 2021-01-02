using System.ComponentModel.DataAnnotations;

namespace DataAccess.DataAnnotation.Unique
{
    public class CompanyValidateUnique : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var baseDbContext = (BaseDbContext)validationContext.GetService(typeof(BaseDbContext));
            var baseDataWork = new BaseDatawork(baseDbContext);

            var idProperty = validationContext.ObjectType.GetProperty("Id");
            //Create
            if (idProperty == null)
            {
                if (baseDataWork.Companies.Any(x => x.VatNumber == value.ToString().Trim()))
                    return new ValidationResult("Το ΑΦΜ πρέπει να είναι μοναδικό");
            }
            //Edit
            else
            {
                var idValue = (int)idProperty
                    .GetValue(validationContext.ObjectInstance);

                var isUnique = !baseDataWork.Companies.Any(x =>
                    x.VatNumber == value.ToString().Trim() &&
                    x.Id != idValue);

                if (!isUnique)
                    return new ValidationResult("Το ΑΦΜ πρέπει να είναι μοναδικό");
            }

            return ValidationResult.Success;
        }
    }
}
