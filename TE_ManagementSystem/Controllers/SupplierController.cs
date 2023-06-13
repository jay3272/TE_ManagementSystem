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

            GlobalValue.LoginUserName = Convert.ToString(Session["UsrName"] ?? "").Trim();

            if (GlobalValue.LoginUserName.ToString().Count() > 0)
            {
                return View();
            }
            else
            {
                return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" }, JsonRequestBehavior.AllowGet);
            }
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
                supplier.Name = supplier.Name.Trim();
                supplier.Email = supplier.Email.Trim();
                supplier.Phone = supplier.Phone.Trim();
                supplier.Address = supplier.Address.Trim();

                try
                {
                    if (Session["UsrName"].ToString().Count() > 0)
                    {
                        supplier.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        supplier.UpdateEmployee = Convert.ToString(Session["UsrName"] ?? "").Trim();
                    }
                    else
                    {
                        return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" });
                }

                db.Suppliers.Add(supplier);

                db.SaveChanges();
                this.logUtil.AppendMethod("Save Create");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" });
            }
        }

        [Authorize(Users = "1,2,3")]
        public ActionResult Edit(int id)
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);

            try
            {
                GlobalValue.LoginUserName = Convert.ToString(Session["UsrName"] ?? "").Trim();

                if (GlobalValue.LoginUserName.ToString().Count() > 0)
                {
                    var model = db.Suppliers.Where(x => x.ID == id).FirstOrDefault();
                    return View(model);
                }
                else
                {
                    return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" });
                }

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
                var model = db.Suppliers.Where(x => x.ID == supplier.ID).FirstOrDefault();
                model.Name = supplier.Name.Trim();
                model.Email = supplier.Email.Trim();
                model.Phone = supplier.Phone.Trim();
                model.Address = supplier.Address.Trim();
                model.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                try
                {
                    if (Session["UsrName"].ToString().Count() > 0)
                    {
                        model.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        model.UpdateEmployee = Convert.ToString(Session["UsrName"] ?? "").Trim();
                    }
                    else
                    {
                        return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" });
                }

                db.SaveChanges();
                this.logUtil.AppendMethod("Save Update");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" });
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