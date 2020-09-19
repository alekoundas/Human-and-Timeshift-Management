using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models.Entity;
using DataAccess.Models.Select2;

namespace WebApplication.Utilities
{
    public class Select2Helper
    {
        public Select2Response CreateSpecializationResponse(IEnumerable<Specialization> specializations, bool hasMore)
        {
            var results = new List<Select2Result>();
            var pegination = new Select2Pagination();

            foreach (var result in specializations)
                results.Add(new Select2Result()
                {
                    id = result.Id,
                    Text = result.Name
                });


            pegination.More = hasMore;
            return new Select2Response() { Results = results, Pagination = pegination };
        }
        public Select2Response CreateLeaveTypeResponse(IEnumerable<LeaveType> leaveTypes, bool hasMore)
        {
            var results = new List<Select2Result>();
            var pegination = new Select2Pagination();

            foreach (var result in leaveTypes)
                results.Add(new Select2Result()
                {
                    id = result.Id,
                    Text = result.Name
                });


            pegination.More = hasMore;
            return new Select2Response() { Results = results, Pagination = pegination };
        }



        public Select2Response CreateCompaniesResponse(IEnumerable<Company> companies, bool hasMore)
        {
            var results = new List<Select2Result>();
            var pegination = new Select2Pagination();

            foreach (var result in companies)
                results.Add(new Select2Result()
                {
                    id = result.Id,
                    Text = result.Title
                });

            pegination.More = hasMore;
            return new Select2Response() { Results = results, Pagination = pegination };

        }

        public Select2Response CreateCustomersResponse(IEnumerable<Customer> customers, bool hasMore)
        {
            var results = new List<Select2Result>();
            var pegination = new Select2Pagination();

            foreach (var result in customers)
                results.Add(new Select2Result()
                {
                    id = result.Id,
                    Text = result.FirstName + " " + result.LastName
                });

            pegination.More = hasMore;
            return new Select2Response() { Results = results, Pagination = pegination };

        }

        public Select2Response CreateWorkplacesResponse(IEnumerable<WorkPlace> workplaces, bool hasMore)
        {
            var results = new List<Select2Result>();
            var pegination = new Select2Pagination();

            foreach (var result in workplaces)
                results.Add(new Select2Result()
                {
                    id = result.Id,
                    Text = result.Title
                });

            pegination.More = hasMore;
            return new Select2Response() { Results = results, Pagination = pegination };
        }

        public Select2Response CreateEmployeesResponse(IEnumerable<Employee> workplaces, bool hasMore)
        {
            var results = new List<Select2Result>();
            var pegination = new Select2Pagination();

            foreach (var result in workplaces)
                results.Add(new Select2Result()
                {
                    id = result.Id,
                    Text = result.FirstName + " " +
                        result.LastName + " - " +
                        result.ErpCode
                });

            pegination.More = hasMore;
            return new Select2Response() { Results = results, Pagination = pegination };

        }
        public Select2Response CreateTimeShiftsResponse(IEnumerable<TimeShift> timeShifts, bool hasMore)
        {
            var results = new List<Select2Result>();
            var pegination = new Select2Pagination();

            foreach (var result in timeShifts)
                results.Add(new Select2Result()
                {
                    id = result.Id,
                    Text = result.WorkPlace.Title + " - " +
                        result.Title + " " +
                        result.Month + "/" +
                        result.Month
                });

            pegination.More = hasMore;
            return new Select2Response() { Results = results, Pagination = pegination };

        }
    }


}
