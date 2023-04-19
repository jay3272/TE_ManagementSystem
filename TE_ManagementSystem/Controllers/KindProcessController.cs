using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;

namespace TE_ManagementSystem.Controllers
{
    public class KindProcessController : Controller
    {
        IKindProcessRepo KindProcessRepo = new KindProcessRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: KindProcess
        [Authorize(Users = "1,2,3,4")]
        public ActionResult Index()
        {
            return View(KindProcessRepo.ListAllKindProcess());
        }

        [Authorize(Users = "1,2")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Users = "1,2")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Number,Spare1,Spare2,Spare3,Spare4,Spare5")] KindProcess kindProcess)
        {
            try
            {
                int maxId = db.KindProcesses.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                maxId += 1;
                kindProcess.ID = maxId;
                db.KindProcesses.Add(kindProcess);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error" });
            }
        }

    }
}