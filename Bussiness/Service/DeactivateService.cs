using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Bussiness.Service
{
    public class DeactivateService
    {
        public static bool CanShowDeactivatedFromUser<T>(HttpContext httpContext) =>
             httpContext.User.Claims.Any(x =>
                x.Value.Contains(typeof(T).Name + "_Deactivate"));
    }
}
