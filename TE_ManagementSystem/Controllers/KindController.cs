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
    public class KindController : Controller
    {
        IKindRepo KindRepo = new KindRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Kind
        [Authorize(Users = "1,2,3,4,5")]
        public ActionResult Index()
        {
            return View(KindRepo.ListAllKind());
        }

        [Authorize(Users = "1,2")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Users = "1,2")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Number,Spare1,Spare2,Spare3,Spare4,Spare5")] Kind kind)
        {
            try
            {
                if (this.CheckInputErr(kind)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }

                int maxId = db.Kinds.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                maxId += 1;
                kind.ID = maxId;
                db.Kinds.Add(kind);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" + ex });
            }
        }

        private bool CheckInputErr(Kind kind)
        {
            if (kind.Name == null || kind.Name == string.Empty) { return true; };
            if (kind.Number == null || kind.Number == string.Empty) { return true; };

            return false;
        }

    }
}