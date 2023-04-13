using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;

namespace TE_ManagementSystem.Controllers
{
    public class KindController : Controller
    {
        IKindRepo KindRepo = new KindRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Kind
        public ActionResult Index()
        {
            return View(KindRepo.ListAllKind());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Number,Spare1,Spare2,Spare3,Spare4,Spare5")] Kind kind)
        {
            int maxId = db.Kinds.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
            maxId += 1;
            kind.ID = maxId;
            db.Kinds.Add(kind);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}