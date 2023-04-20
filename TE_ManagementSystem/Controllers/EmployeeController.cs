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
            try
            {
                if (this.CheckInputErr(employee)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }

                employee.Password = Encryption.Encrypt(employee.Password, "d3A#");
                employee.IsActive = true;
                if (employee.Email == null)
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
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" + ex });
            }
        }

        private bool CheckInputErr(Employee employee)
        {
            if (employee.Opid == null || employee.Opid == string.Empty) { return true; };
            if (employee.Name == null || employee.Name == string.Empty) { return true; };
            if (employee.RankID == 0) { return true; };
            if (employee.DepartmentID == 0) { return true; };

            return false;
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