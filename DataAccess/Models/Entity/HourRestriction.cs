namespace DataAccess.Models.Entity
{
    public class HourRestriction : BaseEntity
    {
        public int Day { get; set; }
        public double MaxTicks { get; set; }

        public int WorkPlaceHourRestrictionId { get; set; }
        public WorkPlaceHourRestriction WorkPlaceHourRestriction { get; set; }
    }
}
