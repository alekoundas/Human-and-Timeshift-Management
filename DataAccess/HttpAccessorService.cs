using Microsoft.AspNetCore.Http;
using System.Linq;

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
    }
}
