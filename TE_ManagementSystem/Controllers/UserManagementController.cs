using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TE_ManagementSystem.Models;

namespace TE_ManagementSystem.Controllers
{
    public class UserManagementController : Controller
    {
        private ManagementContextEntities db = new ManagementContextEntities();
        // GET: UserManagement
        [AllowAnonymous]
        public ActionResult Login()
        {
            Session.Clear();
            Session.Abandon();

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Employee emp)
        {
            if (emp.Password == null) { emp.Password = string.Empty; }
            //emp.Password = Encryption.Encrypt(emp.Password, "d3A#");

            Employee user = db.Employees.SingleOrDefault(usr => ((usr.Opid == emp.Opid) && (usr.Password == emp.Password)));
            if (user != null)
            {
                FormsAuthentication.RedirectFromLoginPage(user.RankID.ToString(), true);
                Session.Add("CurrentUser", user);

                Session.Add("UsrName", user.Name);
                Session.Add("UsrOpid", user.Opid);

                int irank = user.RankID;

                switch (irank)
                {
                    case 1:
                        Session.Add("UsrRank", "Admin");
                        break;
                    case 2:
                        Session.Add("UsrRank", "Supervisor");
                        break;
                    case 3:
                        Session.Add("UsrRank", "Engineer");
                        break;
                    case 4:
                        Session.Add("UsrRank", "Operator");
                        break;
                    case 5:
                        Session.Add("UsrRank", "Guest");
                        break;
                    default:
                        Session.Add("UsrRank", "default");
                        break;
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["message"] = "Password is not valid";
                return RedirectToAction("Login", "UserManagement");
            }

        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Login", "UserManagement");
        }

    }

}