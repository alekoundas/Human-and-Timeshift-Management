using DataAccess.Models.Audit;

namespace DataAccess.Models.Security
{
    public class Log : BaseEntity
    {
        public string OriginalJSON { get; set; }
        public string EditedJSON { get; set; }


        public int LogEntityId { get; set; }
        public LogEntity LogEntity { get; set; }

        public int LogTypeId { get; set; }
        public LogType LogType { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
