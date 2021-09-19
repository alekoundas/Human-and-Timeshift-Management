using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication.Controllers.Security
{
    public class LogController : MasterController
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public LogController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: Logs
        [Authorize(Roles = "Log_View")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Σύνολο Log";
            return View();
        }

        //[HttpGet]
        //public async Task<ActionResult> DownloadExcelWithData()
        //{
        //    var excelColumns = new List<string>(new string[] {
        //        "Name",
        //        "Description",
        //        "IsActive" });

        //    var excelPackage = (await (new ExcelService<Log>(_context)
        //        .CreateNewExcel("Logs"))
        //        .AddSheetAsync(excelColumns, "Logs"))
        //        .CompleteExcel(out var errors);

        //    if (errors.Count == 0)
        //        using (var package = excelPackage)
        //            return File(package.GetAsByteArray(), XlsxContentType, "Logs.xlsx");
        //    else
        //        TempData["StatusMessage"] = "Ωχ! " + string.Join("", errors);

        //    return View();
        //}
    }
}
