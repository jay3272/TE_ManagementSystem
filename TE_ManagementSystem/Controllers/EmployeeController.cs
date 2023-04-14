using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;

namespace TE_ManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        IEmployeeRepo EmployeeRepo = new EmployeeRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Employee
        [Authorize(Users = "1,2,3,4")]
        public ActionResult Index()
        {
            return View(EmployeeRepo.ListAllEmployee());
        }

        [Authorize(Users = "1,2")]
        public ActionResult Create()
        {
            this.loaddefault();
            return View();
        }

        [HttpPost]
        [Authorize(Users = "1,2")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Opid,Name,Email,RankID,DepartmentID,IsActive,Password")] Employee employee)
        {
            employee.Password = Encryption.Encrypt(employee.Password, "d3A#");
            employee.IsActive = true;
            if (employee.Email == string.Empty)
            {
                employee.Email = "NA";
            }
            else
            {
                employee.Email = employee.Email + "@tailyn.com.tw";
            }

            db.Employees.Add(employee);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void loaddefault()
        {
            ViewBag.Department = db.Departments;

            var departmentData = db.Departments;

            List<SelectListItem> selectDepartmentListItems = new List<SelectListItem>();
            List<SelectListItem> selectRankListItems = new List<SelectListItem>();

            foreach (var item in departmentData)
            {
                selectDepartmentListItems.Add(new SelectListItem()
                {
                    Text = item.ID + "/" + item.Name,
                    Value = item.ID.ToString(),
                    Selected = false
                });
            }

            var dictRank = new Dictionary<string, string>();
            dictRank.Add("1", "Admin");
            dictRank.Add("2", "Supervisor");
            dictRank.Add("3", "Engineer");
            dictRank.Add("4", "Guest");

            foreach (KeyValuePair<string, string> item in dictRank)
            {
                selectRankListItems.Add(new SelectListItem()
                {
                    Text = item.Key + "/" + item.Value,
                    Value = item.Key.ToString(),
                    Selected = false
                });
            }
            
            ViewBag.listDepartment = selectDepartmentListItems;
            ViewBag.listRank = selectRankListItems;

        }

    }
}