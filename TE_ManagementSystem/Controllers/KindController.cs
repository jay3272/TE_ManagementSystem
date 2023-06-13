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
    public class KindController : ProjectBase
    {
        IKindRepo KindRepo = new KindRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Kind
        [Authorize(Users = "1,2,3,4,5")]
        public ActionResult Index()
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);
            return View(KindRepo.ListAllKind());
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
        public ActionResult Create([Bind(Include = "ID,Name,Number,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateEmployee")] Kind kind)
        {
            try
            {
                if (this.CheckInputErr(kind)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }

                int maxId = db.Kinds.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                maxId += 1;
                kind.ID = maxId;
                kind.Name = kind.Name.Trim();
                kind.Number = maxId.ToString();

                try
                {
                    if (Session["UsrName"].ToString().Count() > 0)
                    {
                        kind.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        kind.UpdateEmployee = Convert.ToString(Session["UsrName"] ?? "").Trim();
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

                db.Kinds.Add(kind);

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
        //        var model = db.Kinds.Where(x => x.ID == id).FirstOrDefault();

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
        //public ActionResult Edit([Bind(Include = "ID,Name,Number,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateDate,UpdateEmployee")] Kind kind)
        //{
        //    try
        //    {
        //        if (this.CheckInputErr(kind)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }
        //        var model = db.Kinds.Where(x => x.ID == kind.ID).FirstOrDefault();
        //        model.Name = kind.Name;
        //        model.Number = kind.Number;
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

        private bool CheckInputErr(Kind kind)
        {
            if (kind.Name == null || kind.Name == string.Empty) { return true; };
            if (kind.Number == null || kind.Number == string.Empty) { return true; };

            return false;
        }

    }
}