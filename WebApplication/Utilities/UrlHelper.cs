using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Utilities
{
    public static class UrlHelper
    {
        public static string EmployeePerCompany( int employeeId, int companyId)
            => "/api/companies/managecompanyemployees/" + employeeId + "/" + companyId + "/";
        public static string EmployeePerWorkPlace(  int employeeId, int companyId)
            => "/api/workplaces/manageworkplaceemployee/" + employeeId + "/" + companyId + "/";
    }
}
