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
        [Authorize(Users = "1,2,3,4")]
        public ActionResult Index()
        {
            return View(SupplierRepo.ListAllSupplier());
        }
        [Authorize(Users = "1,2")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Users = "1,2")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Email,Phone,Address,Spare2,Spare3,Spare4,Spare5")] Supplier supplier)
        {
            try
            {
                if (this.CheckInputErr(supplier)) { return Json(new { ReturnStatus = "error" }); }

                int maxId = db.Suppliers.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                maxId += 1;
                supplier.ID = maxId;
                db.Suppliers.Add(supplier);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error" });
            }
        }

        private bool CheckInputErr(Supplier supplier)
        {
            if (supplier.Name == null || supplier.Name == string.Empty) { return true; };
            if (supplier.Email == null || supplier.Email == string.Empty) { return true; };
            if (supplier.Phone == null || supplier.Phone == string.Empty) { return true; };
            if (supplier.Address == null || supplier.Address == string.Empty) { return true; };

            return false;
        }

    }
}