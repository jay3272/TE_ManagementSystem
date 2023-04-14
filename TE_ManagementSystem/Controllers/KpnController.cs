using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;

namespace TE_ManagementSystem.Controllers
{
    public class KpnController : Controller
    {
        IKpnRepo KpnRepo = new KpnRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Kpn
        [Authorize(Users = "1,2,3,4")]
        public ActionResult Index()
        {
            return View(KpnRepo.ListAllKpn());
        }

        [Authorize(Users = "1,2")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Users = "1,2")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Spare1,Spare2,Spare3,Spare4,Spare5")] KPN kpn)
        {
            int maxId = db.KPNs.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
            maxId += 1;
            kpn.ID = maxId;
            db.KPNs.Add(kpn);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}