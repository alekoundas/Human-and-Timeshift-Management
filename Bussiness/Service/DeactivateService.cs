using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Bussiness.Service
{
    public class DeactivateService
    {
        //public static ExpressionStarter<T> GetUserFilterDeactivate<T>()
        //{

        //    var filter = PredicateBuilder.New<T>();
        //    filter = filter.And(x => (bool)(x.GetType().GetProperty("IsActive")).GetValue(x) == true);

        //    return filter;
        //}

        public static bool CanShowDeactivatedFromUser<T>(HttpContext httpContext) =>
             httpContext.User.Claims.Any(x =>
                x.Value.Contains(typeof(T).Name + "_Deactivate"));

    }
}
