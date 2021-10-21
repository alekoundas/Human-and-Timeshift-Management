using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class ProjectionController : MasterController
    {
        [Authorize(Roles = "ProjectionDifference_View")]
        public IActionResult Difference()
        {
            ViewData["Title"] = "Διαφορές βαρδιών απο πραγματικές και χρονόγραμμα";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionCurrentDay_View")]
        public IActionResult CurrentDay()
        {
            ViewData["Title"] = "Σύνολο πραγματικών βαρδιών ημέρας";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionConcentric_View")]
        public IActionResult Concentric()
        {
            ViewData["Title"] = "Συνγκεντρωτικό";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionConcentricSpecificDates_View")]
        public IActionResult ConcentricSpecificDates()
        {
            ViewData["Title"] = "Συνγκεντρωτικό με επιλεγμένες ημερομηνίες";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionRealWorkHoursAnalytically_View")]
        public IActionResult RealWorkHoursAnalytically()
        {
            ViewData["Title"] = "Πραγματικές βάρδιες αναλυτικά";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionRealWorkHoursAnalyticallySum_View")]
        public IActionResult RealWorkHoursAnalyticallySum()
        {
            ViewData["Title"] = "Πραγματικές βάρδιες αναλυτικά σε ώρες ";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionRealWorkHoursSpecificDates_View")]
        public IActionResult RealWorkHoursSpecificDates()
        {
            ViewData["Title"] = "Π.Βάρδιες επιλεγμένες ημερομηνίες";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionPresenceDaily_View")]
        public IActionResult PresenceDaily()
        {
            ViewData["Title"] = "Παρουσίες ημέρας";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionEmployeeRealHoursSum_View")]
        public IActionResult EmployeeRealHoursSum()
        {
            ViewData["Title"] = "Ώρες ανα εργαζόμενο";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionRealWorkHourRestrictions_View")]
        public IActionResult RealWorkHourRestrictions()
        {
            ViewData["Title"] = "Έλεγχος υπέρβασης πραγματικών βαρδιών";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionTimeShiftSuggestions_View")]
        public IActionResult TimeShiftSuggestions()
        {
            ViewData["Title"] = "Υποδείξεις Χρονοδιαγράμματος";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionErganiSuggestions_View")]
        public IActionResult ErganiSuggestions()
        {
            ViewData["Title"] = "Υποδείξεις Εργάνη";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionHoursWithComments_View")]
        public IActionResult HoursWithComments()
        {
            ViewData["Title"] = "(Π)Βάρδιες με σχόλια";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            return View();
        }

        [Authorize(Roles = "ProjectionEmployeeConsecutiveDayOff_View")]
        public IActionResult EmployeeConsecutiveDayOff()
        {
            ViewData["Title"] = "Συνεχής ρεπό ανα εργαζόμενο";
            ViewData["Filter"] = "Προαιρετικά φίλτρα αναζήτησης";
            return View();
        }
    }
}
