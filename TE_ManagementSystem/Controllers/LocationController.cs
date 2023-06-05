using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;

namespace TE_ManagementSystem.Controllers
{
    [Authorize]
    public class LocationController : ProjectBase
    {
        ILocationRepo LocationRepo = new LocationRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Location
        [Authorize(Users = "1,2,3,4,5")]
        public ActionResult Index()
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);
            return View(LocationRepo.ListAllLocation());
        }

        [Authorize(Users = "1,2")]
        public ActionResult Create()
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);

            GlobalValuel.LoginUserName = Convert.ToString(Session["UsrName"] ?? "").Trim();

            if (GlobalValuel.LoginUserName.ToString().Count() > 0)
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
        public ActionResult Create([Bind(Include = "ID,Name,RackPosition,Status,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateEmployee")] Location location)
        {
            try
            {
                if (this.CheckInputErr(location)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }

                int maxId = db.Locations.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                maxId += 1;
                location.ID = maxId;

                try
                {
                    if (Session["UsrName"].ToString().Count() > 0)
                    {
                        location.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        location.UpdateEmployee = Convert.ToString(Session["UsrName"] ?? "").Trim();
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

                location.Status = true;
                db.Locations.Add(location);

                db.SaveChanges();
                this.logUtil.AppendMethod("Save Create");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" });
            }
        }

        [Authorize(Users = "1,2")]
        public ActionResult Edit(int id)
        {
            try
            {
                this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);

                GlobalValuel.LoginUserName = Convert.ToString(Session["UsrName"] ?? "").Trim();

                if (GlobalValuel.LoginUserName.ToString().Count() > 0)
                {
                    var model = db.Locations.Where(x => x.ID == id).FirstOrDefault();
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
        [Authorize(Users = "1,2")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,RackPosition,Status,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateDate,UpdateEmployee")] Location location)
        {
            try
            {
                if (this.CheckInputErr(location)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }
                var model = db.Locations.Where(x => x.ID == location.ID).FirstOrDefault();
                model.Name = location.Name;
                model.RackPosition = location.RackPosition;
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

        private bool CheckInputErr(Location location)
        {
            if (location.Name == null || location.Name == string.Empty) { return true; };
            if (location.RackPosition == null || location.RackPosition == string.Empty) { return true; };

            return false;
        }

    }
}