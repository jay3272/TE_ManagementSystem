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
        public ActionResult Index()
        {
            return View(EmployeeRepo.ListAllEmployee());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Email,RankID,DepartmentID,IsActive,Spare1,Spare2,Spare3,Spare4,Spare5")] Employee employee)
        {            
            db.Employees.Add(employee);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}