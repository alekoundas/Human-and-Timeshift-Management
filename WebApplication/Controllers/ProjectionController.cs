using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class ProjectionController : MasterController
    {
        //private readonly BaseDbContext _context;
        //private BaseDatawork _baseDataWork;
        //public ProjectionController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        //{
        //    _context = BaseDbContext;
        //    _baseDataWork = new BaseDatawork(BaseDbContext);
        //}

        [Authorize(Roles = "ProjectionDifference_View")]
        public IActionResult Difference()
        {
            ViewData["Title"] = "Διαφορές βαρδιών απο πραγματικές και χρονόγραμμα";
            ViewData["Filter"] = "Προεραιτικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionConcentric_View")]
        public IActionResult Concentric()
        {
            ViewData["Title"] = "Συνγκεντρωτικό";
            ViewData["Filter"] = "Προεραιτικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionRealWorkHoursAnalytically_View")]
        public IActionResult RealWorkHoursAnalytically()
        {
            ViewData["Title"] = "Πραγματικές βάρδιες αναλυτικά";
            ViewData["Filter"] = "Προεραιτικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionRealWorkHoursAnalyticallySum_View")]
        public IActionResult RealWorkHoursAnalyticallySum()
        {
            ViewData["Title"] = "Πραγματικές βάρδιες αναλυτικά σε ώρες ";
            ViewData["Filter"] = "Προεραιτικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionRealWorkHoursSpecificDates_View")]
        public IActionResult RealWorkHoursSpecificDates()
        {
            ViewData["Title"] = "Επιλεγμένες ημερομηνίες";
            ViewData["Filter"] = "Προεραιτικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionPresenceDaily_View")]
        public IActionResult PresenceDaily()
        {
            ViewData["Title"] = "Παρουσίες ημέρας";
            ViewData["Filter"] = "Προεραιτικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionEmployeeRealHoursSum_View")]
        public IActionResult EmployeeRealHoursSum()
        {
            ViewData["Title"] = "Ώρες ανα εργαζόμενο";
            ViewData["Filter"] = "Προεραιτικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionRealWorkHourRestrictions_View")]
        public IActionResult RealWorkHourRestrictions()
        {
            ViewData["Title"] = "Έλεγχος υπέρβασης πραγματικών βαρδιών";
            ViewData["Filter"] = "Προεραιτικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionTimeShiftSuggestions_View")]
        public IActionResult TimeShiftSuggestions()
        {
            ViewData["Title"] = "Υποδείξεις Χρονοδιαγράμματος";
            ViewData["Filter"] = "Προεραιτικά φίλτρα αναζήτησης";
            return View();
        }
    }
}
