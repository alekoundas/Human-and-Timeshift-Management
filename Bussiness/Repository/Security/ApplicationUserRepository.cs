using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Archium.Security.Core.Repositories;
using Bussiness.Repository;
using Bussiness.Repository.Interface;
using DataAccess;
using DataAccess.Models.Identity;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Business.Repository
{
    public class ApplicationUserRepository : SecurityRepository<ApplicationUser>, IApplicationUserRepository
    {
        protected readonly SecurityDbContext _dbContext;

        public ApplicationUserRepository( SecurityDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public SecurityDbContext SecurityDbContext
        {
            get { return Context as SecurityDbContext; }
        }

        public ApplicationUser Get(string id)
        {
            return SecurityDbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<ApplicationUser>> GetWithPagging(
            Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderingInfo,
            int pageSize = 10,
            int pageIndex = 1)
        {
            if (orderingInfo == null)
            {
                return await SecurityDbContext
                    .Users
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize).ToListAsync();
            }

            var query = (IQueryable<ApplicationUser>)orderingInfo(SecurityDbContext.Users);

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }



        public async Task<ApplicationRole>GetRoleByWorkPlaceAndUser(string workPlaceId,string userId)
        {
            var filter = PredicateBuilder.New<ApplicationRole>();

            var userRoles = await SecurityDbContext.UserRoles
                .Where(x => x.UserId == userId)
                .ToListAsync();

            foreach (var userRole in userRoles)
                filter = filter.Or(x => x.Id == userRole.RoleId && x.WorkPlaceId == workPlaceId);

            return await SecurityDbContext.Roles.FirstOrDefaultAsync(filter);
        }

        public List<ApplicationUser> GetUsersByPredicate(string searchterm)
        {
            return SecurityDbContext.Users.Where(a => a.UserName.Contains(searchterm)).ToList();
        }
        public List<ApplicationUser> GetUsersByPredicateStringSplitted(string searchterm)
        {
            //var dbUsers = _securityDataWork.ApplicationUsers.GetUsersByPredicate(userCredentials.UserName);
            //var dbUser = dbUsers.SingleOrDefault(x => x.UserName.Split('|')[1].Contains(searchterm));
            var dbUsers = SecurityDbContext.Users.Where(a => a.UserName.Contains(searchterm)).ToList();
            return dbUsers.Where(a => a.UserName.Split('|')[1].Contains(searchterm)).ToList();
        }

        public string GetUserMailAddress(string userId)
        {
            return SecurityDbContext.Users.SingleOrDefault(x => x.Id == userId).Email;
        }

        //public List<string> GetRoleUserMail(string roleId)
        //{
        //    List<string> emails = new List<string>();
        //    var listUsers = SecurityDbContext.ApplicationUserRoles.Where(x => x.RoleId == roleId).ToList();
        //    foreach (var listUsersItem in listUsers)
        //    {
        //        var listUsersItemTemp = SecurityDbContext.Users.SingleOrDefault(x => x.Id == listUsersItem.UserId).Email;
        //        emails.Add(listUsersItemTemp);
        //    }
        //    return emails;
        //}

        //public string GetUserFullName(string userId)
        //{
        //    var userProfile = SecurityDbContext.Users.SingleOrDefault(a => a.Id == userId);
        //    return userProfile.Profile.FirstName + " " + userProfile.Profile.LastName;
        //}

        //public ApplicationUserProfile GetUserProfileForDocument(int userId)
        //{
        //    return SecurityDbContext.Users.Where(x => x.Profile.UserId == userId).Select(c => c.Profile).FirstOrDefault();
        //}

        //public ApplicationUser CreateUser(ApplicationUser applicationUser)
        //{
        //    var result = _userManager.Create(applicationUser);
        //    return result.Succeeded ? applicationUser : null;
        //}

      

        //public ApplicationUser FindUserById(int id)
        //{
        //    return _userManager.FindById(id);
        //}

        public void DeleteUser(int id)
        {
            var user = SecurityDbContext.Users.Find(id);
            if (user != null)
            {
                SecurityDbContext.Users.Remove(user);
                SecurityDbContext.SaveChanges();
            }
            else
                throw new Exception("Failed to delete user");
        }
    }
}
