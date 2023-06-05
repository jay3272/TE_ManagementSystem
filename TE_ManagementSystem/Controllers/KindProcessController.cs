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
    public class KindProcessController : ProjectBase
    {
        IKindProcessRepo KindProcessRepo = new KindProcessRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: KindProcess
        [Authorize(Users = "1,2,3,4,5")]
        public ActionResult Index()
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);
            return View(KindProcessRepo.ListAllKindProcess());
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
        public ActionResult Create([Bind(Include = "ID,Name,Number,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateEmployee")] KindProcess kindProcess)
        {
            try
            {
                if (this.CheckInputErr(kindProcess)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }

                int maxId = db.KindProcesses.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                maxId += 1;
                kindProcess.ID = maxId;

                try
                {
                    if (Session["UsrName"].ToString().Count() > 0)
                    {
                        kindProcess.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        kindProcess.UpdateEmployee = Convert.ToString(Session["UsrName"] ?? "").Trim();
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

                db.KindProcesses.Add(kindProcess);

                db.SaveChanges();
                this.logUtil.AppendMethod("Save Create");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" });
            }
        }

        //[Authorize(Users = "1,2,3")]
        //public ActionResult Edit(int id)
        //{
        //    try
        //    {
        //        var model = db.KindProcesses.Where(x => x.ID == id).FirstOrDefault();

        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { ReturnStatus = "error", ReturnData = "Edit(), ex:" + ex });
        //    }
        //}

        //[HttpPost]
        //[Authorize(Users = "1,2,3")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,Name,Number,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateDate,UpdateEmployee")] KindProcess kindProcess)
        //{
        //    try
        //    {
        //        if (this.CheckInputErr(kindProcess)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }
        //        var model = db.KindProcesses.Where(x => x.ID == kindProcess.ID).FirstOrDefault();
        //        model.Name = kindProcess.Name;
        //        model.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //        model.UpdateEmployee = Convert.ToString(Session["UsrName"] ?? "").Trim();

        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" });
        //    }
        //}

        private bool CheckInputErr(KindProcess kindProcess)
        {
            if (kindProcess.Name == null || kindProcess.Name == string.Empty) { return true; };
            if (kindProcess.Number == null || kindProcess.Number == string.Empty) { return true; };

            return false;
        }

    }
}