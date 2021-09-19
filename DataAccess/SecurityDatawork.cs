using DataAccess;
using DataAccess.Repository;
using DataAccess.Repository.Security;
using DataAccess.Repository.Security.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Bussiness
{
    public class SecurityDataWork : ISecurityDatawork
    {
        private readonly SecurityDbContext _dbcontext;
        public IApplicationRoleRepository ApplicationRoles { get; private set; }
        public IApplicationTagRepository ApplicationTags { get; private set; }
        public IApplicationUserRepository ApplicationUsers { get; private set; }
        public IApplicationUserRoleRepository ApplicationUserRoles { get; private set; }
        public IApplicationUserTagRepository ApplicationUserTags { get; private set; }
        public INotificationRepository Notifications { get; private set; }
        public ILogRepository Logs { get; private set; }
        public ILogTypeRepository LogTypes { get; private set; }
        public ILogEntityRepository LogEntities { get; private set; }

        public SecurityDataWork(SecurityDbContext context)
        {
            _dbcontext = context;

            ApplicationRoles = new ApplicationRoleRepository(_dbcontext);
            ApplicationTags = new ApplicationTagRepository(_dbcontext);
            ApplicationUsers = new ApplicationUserRepository(_dbcontext);
            ApplicationUserRoles = new ApplicationUserRoleRepository(_dbcontext);
            ApplicationUserTags = new ApplicationUserTagRepository(_dbcontext);
            Notifications = new NotificationRepository(_dbcontext);
            Logs = new LogRepository(_dbcontext);
            LogTypes = new LogTypeRepository(_dbcontext);
            LogEntities = new LogEntityRepository(_dbcontext);
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _dbcontext.SaveChangesAsync();
        }

        public void Update<TEntity>(TEntity model)
        {
            _dbcontext.Entry(model).State = EntityState.Modified;
        }

        public void UpdateRange<TEntity>(List<TEntity> models)
        {
            foreach (var model in models)
                _dbcontext.Entry(model).State = EntityState.Modified;
        }

        public void Dispose()
        {
            _dbcontext.Dispose();
        }

    }
}
