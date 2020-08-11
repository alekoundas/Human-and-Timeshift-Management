using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bussiness;
using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class ProjectionController : Controller
    {
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public ProjectionController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        public IActionResult Difference()
        {
            return View();
        }

    }
}
