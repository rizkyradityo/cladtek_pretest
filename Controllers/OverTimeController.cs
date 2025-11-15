using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PTXYZ_OvertimeApp.Models;

namespace PTXYZ_OvertimeApp.Controllers
{
    public class OverTimeController : Controller
    {
        private PTXYZContext db = new PTXYZContext();

        // GET: OverTime
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            // Using EF6 Raw SQL Query to get overtime with employee info
            string sql = @"SELECT ot.*, e.FullName as EmployeeName, e.NIK
                          FROM OverTime ot 
                          INNER JOIN Employee e ON ot.EmployeeId = e.EmployeeId 
                          ORDER BY ot.Date DESC, ot.OverTimeId DESC";
            
            var overtimes = db.Database.SqlQuery<OverTimeViewModel>(sql).ToList();

            // Pagination
            int totalRecords = overtimes.Count;
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            
            var pagedOvertimes = overtimes
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(pagedOvertimes);
        }

        // GET: OverTime/Create
        public ActionResult Create()
        {
            // Using ViewBag to pass employee data from controller to view
            var employees = db.Employees
                .OrderBy(e => e.FullName)
                .Select(e => new { e.EmployeeId, DisplayText = e.NIK + " - " + e.FullName })
                .ToList();
            ViewBag.Employees = employees;

            return View();
        }

        // POST: OverTime/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(OverTime overtime)
        {
            if (ModelState.IsValid)
            {
                // Validate Time Finish is after Time Start
                if (overtime.TimeFinish <= overtime.TimeStart)
                {
                    return Json(new { success = false, message = "Time Finish must be later than Time Start!" });
                }

                // Validate maximum OT hours (should be done on client side, but double-check here)
                if (overtime.ActualOTHours > 3)
                {
                    return Json(new { success = false, message = "Maximum Actual OT Hours is 3 hours!" });
                }

                db.OverTimes.Add(overtime);
                db.SaveChanges();
                return Json(new { success = true, message = "Overtime entry created successfully!" });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, message = string.Join("<br/>", errors) });
        }

        // GET: OverTime/Edit/5
        public ActionResult Edit(int id)
        {
            var overtime = db.OverTimes.Find(id);
            if (overtime == null)
            {
                return HttpNotFound();
            }

            return Json(new
            {
                overTimeId = overtime.OverTimeId,
                employeeId = overtime.EmployeeId,
                date = overtime.Date.ToString("yyyy-MM-dd"),
                timeStart = overtime.TimeStart.ToString("yyyy-MM-dd HH:mm"),
                timeFinish = overtime.TimeFinish.ToString("yyyy-MM-dd HH:mm"),
                actualOTHours = overtime.ActualOTHours,
                calculatedOTHours = overtime.CalculatedOTHours,
                remarks = overtime.Remarks
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: OverTime/Edit
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(OverTime overtime)
        {
            if (ModelState.IsValid)
            {
                // Validate Time Finish is after Time Start
                if (overtime.TimeFinish <= overtime.TimeStart)
                {
                    return Json(new { success = false, message = "Time Finish must be later than Time Start!" });
                }

                // Validate maximum OT hours
                if (overtime.ActualOTHours > 3)
                {
                    return Json(new { success = false, message = "Maximum Actual OT Hours is 3 hours!" });
                }

                db.Entry(overtime).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, message = "Overtime entry updated successfully!" });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, message = string.Join("<br/>", errors) });
        }

        // POST: OverTime/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var overtime = db.OverTimes.Find(id);
            if (overtime == null)
            {
                return Json(new { success = false, message = "Overtime entry not found!" });
            }

            db.OverTimes.Remove(overtime);
            db.SaveChanges();
            return Json(new { success = true, message = "Overtime entry deleted successfully!" });
        }

        // GET: OverTime/GetEmployees (for AJAX)
        public ActionResult GetEmployees()
        {
            var employees = db.Employees
                .OrderBy(e => e.FullName)
                .Select(e => new { value = e.EmployeeId, text = e.NIK + " - " + e.FullName })
                .ToList();

            return Json(employees, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    // ViewModel for OverTime with Employee Name
    public class OverTimeViewModel
    {
        public int OverTimeId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string NIK { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeFinish { get; set; }
        public decimal ActualOTHours { get; set; }
        public decimal CalculatedOTHours { get; set; }
        public string Remarks { get; set; }
    }
}
