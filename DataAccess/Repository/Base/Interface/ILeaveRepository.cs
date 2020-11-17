using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repository.Interface;
using DataAccess.Models.Entity;

namespace DataAccess.Repository.Base.Interface
{
    public interface ILeaveRepository : IBaseRepository<Leave>
    {
        Task<List<Leave>> GetCurrentAssignedOnCell(int year, int month, int day, int employeeId);
    }
}
