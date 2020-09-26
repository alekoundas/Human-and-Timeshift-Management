using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Repository.Interface;
using DataAccess.Models.Entity;

namespace Bussiness.Repository.Base.Interface
{
    public interface ILeaveRepository : IBaseRepository<Leave>
    {
        Task<List<Leave>> GetCurrentAssignedOnCell(int year, int month, int day, int employeeId);
    }
}
