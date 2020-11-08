namespace DataAccess.Models.Entity
{
    public class HourRestriction : BaseEntityIsActive
    {
        public int Day { get; set; }
        public double MaxTicks { get; set; }

        public int WorkPlaceHourRestrictionId { get; set; }
        public WorkPlaceHourRestriction WorkPlaceHourRestriction { get; set; }
    }
}
