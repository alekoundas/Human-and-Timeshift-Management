using DataAccess.Models.Audit;
using System.Collections.Generic;

namespace DataAccess.Models.Security
{
    public class LogType : BaseEntityIsActive
    {
        public string Title { get; set; }
        public string Title_GR { get; set; }

        public List<Log> Logs { get; set; }
    }
}
