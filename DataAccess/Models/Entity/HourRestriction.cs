namespace DataAccess.Models.Entity
{
    public class HourRestriction : BaseEntity
    {
        public int Day { get; set; }
        public int MaxHours { get; set; }

        public int WorkPlaceHourRestrictionId { get; set; }
        public WorkPlaceHourRestriction WorkPlaceHourRestriction { get; set; }
    }
}
