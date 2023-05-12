using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;

namespace TE_ManagementSystem.Controllers
{
    [Authorize]
    public class SupplierController : ProjectBase
    {
        ISupplierRepo SupplierRepo = new SupplierRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Supplier
        [Authorize(Users = "1,2,3,4,5")]
        public ActionResult Index()
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);
            return View(SupplierRepo.ListAllSupplier());
        }
        [Authorize(Users = "1,2")]
        public ActionResult Create()
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);
            return View();
        }

        [HttpPost]
        [Authorize(Users = "1,2")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Email,Phone,Address,Spare2,Spare3,Spare4,Spare5,UpdateEmployee")] Supplier supplier)
        {
            try
            {
                if (this.CheckInputErr(supplier)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }

                int maxId = db.Suppliers.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                maxId += 1;
                supplier.ID = maxId;
                supplier.UpdateEmployee = Session["UsrName"].ToString();

                db.Suppliers.Add(supplier);

                db.SaveChanges();
                this.logUtil.AppendMethod("Save Create");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" + ex });
            }
        }

        [Authorize(Users = "1,2,3")]
        public ActionResult Edit(int id)
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);

            try
            {
                var model = db.Suppliers.Where(x => x.ID == id).FirstOrDefault();

                return View(model);
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "Edit(), ex:" + ex });
            }
        }

        [HttpPost]
        [Authorize(Users = "1,2,3")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Email,Phone,Address,Spare2,Spare3,Spare4,Spare5,UpdateDate,UpdateEmployee")] Supplier supplier)
        {
            try
            {
                if (this.CheckInputErr(supplier)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }

                supplier.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                supplier.UpdateEmployee = Session["UsrName"].ToString();

                db.SaveChanges();
                this.logUtil.AppendMethod("Save Update");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" + ex });
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