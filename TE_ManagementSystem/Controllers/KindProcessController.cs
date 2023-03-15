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
        public ActionResult Index()
        {
            return View(KindProcessRepo.ListAllKindProcess());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Number,Spare1,Spare2,Spare3,Spare4,Spare5")] KindProcess kindProcess)
        {
            db.KindProcesses.Add(kindProcess);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}