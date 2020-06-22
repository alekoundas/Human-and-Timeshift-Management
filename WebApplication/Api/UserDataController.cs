using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Repository;
using DataAccess;
using DataAccess.Models;
using DataAccess.Models.Datatable;
using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApplication.Api
{
    [Route("api/users/")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly BaseDbContext _baseDbContext;
        public UserDataController(SignInManager<ApplicationUser> signInManager,
          UserManager<ApplicationUser> userManager,
          BaseDbContext BaseDbContext)
        {
            _baseDbContext = BaseDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("get")]
        public async Task<ActionResult<Employee>> Get([FromBody] Datatable datatable)
        {
            var total = _userManager.Users.Count();
            var filteredCount = total;
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var isDescending = datatable.Order[0].Dir == "desc";

            BaseDatawork zzz = new BaseDatawork(_baseDbContext);
            var zzzzz =  zzz.Employees;
            ICollection<ApplicationUser> applicationUsers =
                await _userManager.Users.ToListAsync();
            var response = new DatatableResponse<ApplicationUser>
            {
                data = applicationUsers,
                draw = datatable.Draw,
                recordsFiltered = filteredCount,
                recordsTotal = total
            };

            return Ok(response);
        }
    }
}
