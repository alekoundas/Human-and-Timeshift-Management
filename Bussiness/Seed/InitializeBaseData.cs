using DataAccess;
using DataAccess.Models.Entity;
using System;
using System.Linq;

namespace Bussiness.Seed
{
    public class InitializeBaseData
    {
        public static void SeedData(BaseDbContext context)
        {
            if (!context.ContractTypes.Any(x => x.Name == "Άλλο"))
                context.Add(new ContractType
                {
                    Name = "Άλλο",
                    IsActive = true,
                    CreatedOn = DateTime.Now
                });

            context.SaveChanges();
        }

    }
}
