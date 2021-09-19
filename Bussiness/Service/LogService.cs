using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bussiness.Service
{
    public class LogService
    {
        private readonly SecurityDbContext _dbContext;
        private readonly SecurityDataWork _securityDatawork;

        public LogService(SecurityDbContext dbContext)
        {
            _dbContext = dbContext;
            _securityDatawork = new SecurityDataWork(dbContext);
        }

        public void OnCreateEntity(string entityName, object originalEntity)
        {
            var logTypeId = _securityDatawork.LogTypes.Query.FirstOrDefault(x => x.Title == "Create").Id;
            var logEntityId = _securityDatawork.LogEntities.Query.FirstOrDefault(x => x.Title== entityName).Id;
            if (logEntityId != 0 && logEntityId != 0)
                _securityDatawork.Logs.Add(new Log
                {
                    LogEntityId = logEntityId,
                    LogTypeId = logTypeId,
                    EditedJSON = "",
                    OriginalJSON = Newtonsoft.Json.JsonConvert.SerializeObject(originalEntity),
                    CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                    CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                    CreatedOn = DateTime.Now
                });
            _dbContext.SaveChanges();

        }


        public void OnDeleteEntity(string entityName, object originalEntity)
        {
            var logTypeId = _securityDatawork.LogTypes.Query.FirstOrDefault(x => x.Title == "Delete").Id;
            var logEntityId = _securityDatawork.LogEntities.Query.FirstOrDefault(x => x.Title == entityName).Id;
            if (logEntityId != 0 && logEntityId != 0)
                _securityDatawork.Logs.Add(new Log
                {
                    LogEntityId = logEntityId,
                    LogTypeId = logTypeId,
                    EditedJSON = "",
                    OriginalJSON = Newtonsoft.Json.JsonConvert.SerializeObject(originalEntity),
                    CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                    CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                    CreatedOn = DateTime.Now
                });

            _dbContext.SaveChanges();
        }

        public void OnEditEntity(string entityName, object originalEntity, object editedEntity)
        {
            var logTypeId = _securityDatawork.LogTypes.Query.FirstOrDefault(x => x.Title == "Edit").Id;
            var logEntityId = _securityDatawork.LogEntities.Query.FirstOrDefault(x => x.Title == entityName).Id;
            if (logEntityId != 0 && logEntityId != 0)
                _securityDatawork.Logs.Add(new Log
                {
                    LogEntityId = logEntityId,
                    LogTypeId = logTypeId,
                    EditedJSON = Newtonsoft.Json.JsonConvert.SerializeObject(editedEntity),
                    OriginalJSON = Newtonsoft.Json.JsonConvert.SerializeObject(originalEntity),
                    CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                    CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                    CreatedOn = DateTime.Now
                });
            _dbContext.SaveChanges();

        }
    }
}
