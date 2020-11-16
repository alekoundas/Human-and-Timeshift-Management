namespace Bussiness.Helpers
{
    public static class UrlHelper
    {
        public static string EmployeeCompany(int employeeId, int companyId)
            => "/api/companies/managecompanyemployees/" + employeeId + "/" + companyId + "/";
        public static string EmployeeWorkPlace(int employeeId, int workPlaceId)
            => "/api/workplaces/manageworkplaceemployee/" + employeeId + "/" + workPlaceId + "/";

        public static string CustomerWorkPlace(int customerId, int workPlaceId)
            => "/api/workplaces/manageworkplacecustomer/" + customerId + "/" + workPlaceId + "/";
    }
}
