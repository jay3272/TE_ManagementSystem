using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;

namespace TE_ManagementSystem.Controllers
{
    public class UserManagementController : Controller
    {
        private ManagementContextEntities db = new ManagementContextEntities();
        // GET: UserManagement
        public ActionResult Login()
        {
            Session.Clear();
            Session.Abandon();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Employee emp)
        {
            if (emp.Password == null) { emp.Password = string.Empty; }
            emp.Password = Encryption.Encrypt(emp.Password, "d3A#");

            Employee user = db.Employees.SingleOrDefault(usr => ((usr.Opid == emp.Opid) && (usr.Password == emp.Password)));
            if (user != null)
            {
                Session.Add("CurrentUser", user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["message"] = "Password is not valid";
                return RedirectToAction("Login", "UserManagement");
            }
    
    }

    }

}