using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;

namespace TE_ManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        ICustomerRepo CustomerRepo = new CustomerRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Customer
        public ActionResult Index()
        {
            return View(CustomerRepo.ListAllCustomer());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,CustCode,Spare1,Spare2,Spare3,Spare4,Spare5")] Customer customer)
        {
            int maxId = db.Customers.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
            maxId += 1;
            customer.ID = maxId;

            db.Customers.Add(customer);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}