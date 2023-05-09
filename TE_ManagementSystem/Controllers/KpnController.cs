using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;

namespace TE_ManagementSystem.Controllers
{
    [Authorize]
    public class KpnController : Controller
    {
        IKpnRepo KpnRepo = new KpnRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Kpn
        [Authorize(Users = "1,2,3,4,5")]
        public ActionResult Index()
        {
            return View(KpnRepo.ListAllKpn());
        }

        [Authorize(Users = "1,2,3")]
        public ActionResult Create()
        {
            return View();
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
                kpn.UpdateEmployee = Session["UsrName"].ToString();

                db.KPNs.Add(kpn);

                db.SaveChanges();
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
            try
            {
                var model = db.KPNs.Where(x => x.ID == id).FirstOrDefault();

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
        public ActionResult Edit([Bind(Include = "ID,Name,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateDate,UpdateEmployee")] KPN kpn)
        {
            try
            {
                if (this.CheckInputErr(kpn)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }

                kpn.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                kpn.UpdateEmployee = Session["UsrName"].ToString();

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" + ex });
            }
        }

        private bool CheckInputErr(KPN kpn)
        {
            if (kpn.Name == null || kpn.Name == string.Empty) { return true; };

            return false;
        }

    }
}