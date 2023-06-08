using Castle.Core.Resource;
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
    public class KpnController : ProjectBase
    {
        IKpnRepo KpnRepo = new KpnRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Kpn
        [Authorize(Users = "1,2,3,4,5")]
        public ActionResult Index()
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);
            return View(KpnRepo.ListAllKpn());
        }

        [Authorize(Users = "1,2,3")]
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
        [Authorize(Users = "1,2,3")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateEmployee")] KPN kpn)
        {
            try
            {
                if (this.CheckInputErr(kpn)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }

                int maxId = db.KPNs.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                maxId += 1;
                kpn.ID = maxId;

                try
                {
                    if (Session["UsrName"].ToString().Count() > 0)
                    {
                        kpn.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        kpn.UpdateEmployee = Convert.ToString(Session["UsrName"] ?? "").Trim();
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

                db.KPNs.Add(kpn);

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
            try
            {
                this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);

                GlobalValue.LoginUserName = Convert.ToString(Session["UsrName"] ?? "").Trim();

                if (GlobalValue.LoginUserName.ToString().Count() > 0)
                {
                    var model = db.KPNs.Where(x => x.ID == id).FirstOrDefault();
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
        public ActionResult Edit([Bind(Include = "ID,Name,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateDate,UpdateEmployee")] KPN kpn)
        {
            try
            {
                if (this.CheckInputErr(kpn)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }
                var model = db.KPNs.Where(x => x.ID == kpn.ID).FirstOrDefault();
                model.Name = kpn.Name;
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

        private bool CheckInputErr(KPN kpn)
        {
            if (kpn.Name == null || kpn.Name == string.Empty) { return true; };

            return false;
        }

    }
}