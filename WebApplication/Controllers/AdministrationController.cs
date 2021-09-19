using DataAccess;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    public class AdministrationController : MasterController
    {
        private readonly BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public AdministrationController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // GET: Administration/BatchTimeshiftCreate
        [Authorize(Roles = "AdministrationBatchTimeshiftCreate_View")]
        public IActionResult BatchTimeshiftCreate()
        {
            ViewData["Title"] = "Μαζική δημιουργια χρονοδιαγραμμάτών";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            ViewData["Form"] = "Χρονοδιαγράμματα προς μαζική εισαγωγή";
            return View();
        }

        // POST:Administration/BatchTimeshiftCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatchTimeshiftCreate(TimeShiftBatchCreate timeshiftBatch)
        {
            if (ModelState.IsValid)
            {
                var timeShifts = TimeShiftBatchCreate.CreateFrom(timeshiftBatch);
                _baseDataWork.TimeShifts.AddRange(timeShifts);

                var status = await _baseDataWork.SaveChangesAsync();
                if (status > 0)
                    TempData["StatusMessage"] = "Δημιουργήθηκαν με επιτυχία " +
                        timeShifts.Count +
                    " χρονοδιαγράμματα";
                else
                    TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";

            }
            ViewData["Title"] = "Μαζική δημιουργια χρονοδιαγραμμάτών";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            ViewData["Form"] = "Χρονοδιαγράμματα προς μαζική εισαγωγή";
            return View();
        }
    }
}
