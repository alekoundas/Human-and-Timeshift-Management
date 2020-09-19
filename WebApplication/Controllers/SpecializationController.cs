using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using DataAccess.ViewModels;
using Bussiness;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using WebApplication.Utilities;

namespace WebApplication.Controllers
{
    public class SpecializationController : Controller
    {
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private HostingEnvironment _hostingEnvironment;
        public SpecializationController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _hostingEnvironment = new HostingEnvironment();
        }

        // GET: Specializations
        [Authorize(Roles = "Specialization_View")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Σύνολο ειδικοτήτων";
            return View();
        }

        // GET: Specializations/Details/5
        [Authorize(Roles = "Specialization_View")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var specialization = await _baseDataWork.Specializations.FindAsync((int)id);
            if (specialization == null)
                return NotFound();

            ViewData["Title"] = "Προβολή ειδικότητας";
            return View(specialization);
        }

        // GET: Specializations/Create
        [Authorize(Roles = "Specialization_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη ειδικότητας";

            return View();
        }

        // POST: Specializations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecializationCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _baseDataWork.Specializations.Add(
                    SpecializationCreateViewModel.CreateFrom(viewModel));
                await _baseDataWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Specializations/Edit/5
        [Authorize(Roles = "Specialization_Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var specialization = await _baseDataWork.Specializations.FindAsync((int)id);
            if (specialization == null)
                return NotFound();

            ViewData["Title"] = "Επεξεργασία ειδικότητας";
            return View(specialization);
        }

        // POST: Specializations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Specialization specialization)
        {
            if (id != specialization.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _baseDataWork.Update(specialization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecializationExists(specialization.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(specialization);
        }


        //public ActionResult Download()
        //{
        //    string Files = "wwwroot/ExcelTemplates/SpecializationTemplate.xlsx";
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
        //    System.IO.File.WriteAllBytes(Files, fileBytes);
        //    MemoryStream ms = new MemoryStream(fileBytes);
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "employee.xlsx");
        //}
        public async Task<ActionResult> DownloadAsync()
        {
            var excelHelper = new ExcelHelper();


            //var workbook = excelHelper.CreateNewExcel()
            //            .AddSheet("Specializations",
            //                new List<string>(new string[] { "Name", "Description" }))
            //            .Workbook
                      
            //          ;
            

                    HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFFont myFont = (HSSFFont)workbook.CreateFont();
            myFont.FontHeightInPoints = 11;
            myFont.FontName = "Tahoma";


            // Defining a border
            HSSFCellStyle borderedCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            borderedCellStyle.SetFont(myFont);
            borderedCellStyle.BorderLeft = BorderStyle.Medium;
            borderedCellStyle.BorderTop = BorderStyle.Medium;
            borderedCellStyle.BorderRight = BorderStyle.Medium;
            borderedCellStyle.BorderBottom = BorderStyle.Medium;
            borderedCellStyle.VerticalAlignment = VerticalAlignment.Center;

            ISheet Sheet = workbook.CreateSheet("Report");
            //Creat The Headers of the excel
            IRow HeaderRow = Sheet.CreateRow(0);

            //Create The Actual Cells
            excelHelper.CreateCell(HeaderRow, 0, "Batch Name", borderedCellStyle);
            excelHelper.CreateCell(HeaderRow, 1, "RuleID", borderedCellStyle);
            excelHelper.CreateCell(HeaderRow, 2, "Rule Type", borderedCellStyle);
            excelHelper.CreateCell(HeaderRow, 3, "Code Message Type", borderedCellStyle);
            excelHelper.CreateCell(HeaderRow, 4, "Severity", borderedCellStyle);


            string fileName = "SpecializationTemplate.xlsx";
            string folderPath = "wwwroot/ExcelTemplates";
            string datafile = Path.Combine(folderPath, fileName);
            FileStream file = new FileStream(datafile, FileMode.CreateNew);
            workbook.Write(file);

            var memoryStream = new MemoryStream();
            using (var fileStream = new FileStream(Path.Combine(folderPath, fileName), FileMode.Open))
            {
                await fileStream.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;
            return File(memoryStream, 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
        public ActionResult Import()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "UploadExcel";
            string webRootPath = _hostingEnvironment.ContentRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    sb.Append("<table class='table table-bordered'><tr>");
                    for (int j = 0; j < cellCount; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                        sb.Append("<th>" + cell.ToString() + "</th>");
                    }
                    sb.Append("</tr>");
                    sb.AppendLine("<tr>");
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                        }
                        sb.AppendLine("</tr>");
                    }
                    sb.Append("</table>");
                }
            }
            return this.Content(sb.ToString());
        }
        private bool SpecializationExists(int id)
        {
            return _context.Specializations.Any(e => e.Id == id);
        }
    }
}
