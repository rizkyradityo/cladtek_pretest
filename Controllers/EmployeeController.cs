using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using PTXYZ_OvertimeApp.Models;

namespace PTXYZ_OvertimeApp.Controllers
{
    public class EmployeeController : Controller
    {
        private PTXYZContext db = new PTXYZContext();

        // GET: Employee
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            // Using EF6 Raw SQL Query to get employees with department info
            string sql = @"SELECT e.*, d.DepartmentName 
                          FROM Employee e 
                          INNER JOIN Department d ON e.DepartmentId = d.DepartmentId 
                          ORDER BY e.EmployeeId";
            
            var employees = db.Database.SqlQuery<EmployeeViewModel>(sql).ToList();

            // Pagination
            int totalRecords = employees.Count;
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            
            var pagedEmployees = employees
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(pagedEmployees);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            // Using ViewBag to pass department data from controller to view
            var departments = db.Departments.OrderBy(d => d.DepartmentName).ToList();
            ViewBag.Departments = departments;

            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicate NIK using LINQ
                var existingEmployee = db.Employees
                    .Where(e => e.NIK == employee.NIK)
                    .FirstOrDefault();

                if (existingEmployee != null)
                {
                    return Json(new { success = false, message = "NIK already exists! Please use a unique NIK." });
                }

                db.Employees.Add(employee);
                db.SaveChanges();
                return Json(new { success = true, message = "Employee created successfully!" });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, message = string.Join("<br/>", errors) });
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            var employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            // Using ViewBag to pass department data
            var departments = db.Departments.OrderBy(d => d.DepartmentName).ToList();
            ViewBag.Departments = departments;

            return Json(new
            {
                employeeId = employee.EmployeeId,
                nik = employee.NIK,
                fullName = employee.FullName,
                departmentId = employee.DepartmentId,
                position = employee.Position,
                laptopAllowance = employee.LaptopAllowance,
                mealAllowance = employee.MealAllowance,
                address = employee.Address,
                phoneNumber = employee.PhoneNumber,
                joinDate = employee.JoinDate?.ToString("yyyy-MM-dd")
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: Employee/Edit
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicate NIK using LINQ (excluding current employee)
                var existingEmployee = db.Employees
                    .Where(e => e.NIK == employee.NIK && e.EmployeeId != employee.EmployeeId)
                    .FirstOrDefault();

                if (existingEmployee != null)
                {
                    return Json(new { success = false, message = "NIK already exists! Please use a unique NIK." });
                }

                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, message = "Employee updated successfully!" });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, message = string.Join("<br/>", errors) });
        }

        // POST: Employee/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var employee = db.Employees.Find(id);
            if (employee == null)
            {
                return Json(new { success = false, message = "Employee not found!" });
            }

            // Check if employee has overtime entries using LINQ
            var hasOvertime = db.OverTimes.Any(o => o.EmployeeId == id);

            if (hasOvertime)
            {
                return Json(new { success = false, message = "Cannot delete employee! This employee has overtime entries." });
            }

            db.Employees.Remove(employee);
            db.SaveChanges();
            return Json(new { success = true, message = "Employee deleted successfully!" });
        }

        // GET: Employee/GetDepartments (for AJAX)
        public ActionResult GetDepartments()
        {
            var departments = db.Departments
                .OrderBy(d => d.DepartmentName)
                .Select(d => new { value = d.DepartmentId, text = d.DepartmentName })
                .ToList();

            return Json(departments, JsonRequestBehavior.AllowGet);
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

    // ViewModel for Employee with Department Name
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string NIK { get; set; }
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Position { get; set; }
        public bool LaptopAllowance { get; set; }
        public bool MealAllowance { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? JoinDate { get; set; }
    }
}
