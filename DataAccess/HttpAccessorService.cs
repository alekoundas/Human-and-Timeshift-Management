using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DataAccess
{
    public static class HttpAccessorService
    {
        public static string GetLoggeInUser_FullName
        {
            get => new HttpContextAccessor().HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "LastName").Value +
                " - " +
                new HttpContextAccessor().HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == "FirstName").Value +
                "";
        }

        public static string GetLoggeInUser_Id
        {
            get => new HttpContextAccessor().HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "UserID")?.Value;
        }



        public static List<Claim> GetLoggeInUser_Claims()
        {
            return new HttpContextAccessor().HttpContext.User.Claims.ToList();
        }

        public static List<ClaimsIdentity> GetLoggeInUser_Identities()
        {
            return new HttpContextAccessor().HttpContext.User.Identities.ToList();
        }
    }
}
