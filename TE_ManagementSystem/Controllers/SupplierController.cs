using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;

namespace TE_ManagementSystem.Controllers
{
    public class SupplierController : Controller
    {
        ISupplierRepo SupplierRepo = new SupplierRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Supplier
        public ActionResult Index()
        {
            return View(SupplierRepo.ListAllSupplier());
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Email,Phone,Address,Spare2,Spare3,Spare4,Spare5")] Supplier supplier)
        {
            db.Suppliers.Add(supplier);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}