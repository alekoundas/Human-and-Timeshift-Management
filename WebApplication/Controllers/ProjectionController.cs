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
            ViewData["Title"] = "Διαφορές βαρδιών απο πραγματικές και χρονόγραμμα";
            ViewData["Filter"] = "Προεραιτκά φίλτρα αναζήτησης";
            return View();
        }

        public IActionResult Concentric()
        {
            ViewData["Title"] = "Συνγκεντρωτικό";
            ViewData["Filter"] = "Προεραιτκά φίλτρα αναζήτησης";
            return View();
        }
        public IActionResult RealWorkHoursAnalytically()
        {
            ViewData["Title"] = "Πραγματικές βάρδιες αναλυτικά";
            ViewData["Filter"] = "Προεραιτκά φίλτρα αναζήτησης";
            return View();
        }

        public IActionResult RealWorkHoursAnalyticallySum()
        {
            ViewData["Title"] = "Πραγματικές βάρδιες αναλυτικά σε ώρες ";
            ViewData["Filter"] = "Προεραιτκά φίλτρα αναζήτησης";
            return View();
        }

        public IActionResult RealWorkHoursSpecificDates()
        {
            ViewData["Title"] = "Επιλεγμένες ημερομηνίες";
            ViewData["Filter"] = "Προεραιτκά φίλτρα αναζήτησης";
            return View();
        }
        public IActionResult PresenceDaily()
        {
            ViewData["Title"] = "Παρουσίες ημέρας";
            ViewData["Filter"] = "Προεραιτκά φίλτρα αναζήτησης";
            return View();
        } 
        public IActionResult EmployeeRealHoursSum()
        {
            ViewData["Title"] = "Ώρες ανα εργαζόμενο";
            ViewData["Filter"] = "Προεραιτκά φίλτρα αναζήτησης";
            return View();
        }

    }
}
