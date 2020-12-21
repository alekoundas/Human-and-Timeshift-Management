using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.Services;
using LinqKit;
using System.Collections.Generic;
using System.Linq;

namespace Bussiness.Service
{
    //TEntity = RealWorkHours || WorkHours
    public class ValidationService
    {
        private BaseDatawork _baseDatawork { get; }
        private ICollection<RealWorkHour> _realWorkHours { get; }

        public ValidationService()
        {

        }
        public ValidationService(BaseDatawork baseDatawork)
        {
            _baseDatawork = baseDatawork;
        }
        public ValidationService(ICollection<RealWorkHour> realWorkHours)
        {
            _realWorkHours = realWorkHours;
        }

        public ValidationResult<RealWorkHour> AreDatesOvertime(ICollection<RealWorkHour> realWorkHours)
        {

            var validationResult = new ValidationResult<RealWorkHour>();
            //var validationResult = new Dictionary<RealWorkHour, List<string>>();

            foreach (var realWorkHour in realWorkHours)
            {
                var filter = PredicateBuilder.New<RealWorkHour>();
                var filterOr = PredicateBuilder.New<RealWorkHour>();

                var startOn = realWorkHour.StartOn.AddHours(-11);
                var endOn = realWorkHour.EndOn.AddHours(11);

                filterOr = filterOr.Or(x => x.StartOn <= startOn && startOn <= x.EndOn);
                filterOr = filterOr.Or(x => x.StartOn <= endOn && endOn <= x.EndOn);
                filterOr = filterOr.Or(x => startOn < x.StartOn && x.EndOn < endOn);

                //Exlude current
                filter = filter.And(x =>
                    x.StartOn != realWorkHour.StartOn && x.EndOn != realWorkHour.EndOn);


                filter = filter.And(x => x.StartOn.Year == realWorkHour.StartOn.Year);
                filter = filter.And(x => x.StartOn.Month == realWorkHour.StartOn.Month);
                filter = filter.And(filterOr);
                var skata = realWorkHours.Where(filter).ToList();
                if (skata.Count > 0)
                {

                    var zzz = "";
                }

            }

            return validationResult;
        }


    }
}
