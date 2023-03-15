using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;

namespace TE_ManagementSystem.Controllers
{
    public class LocationController : Controller
    {
        ILocationRepo LocationRepo = new LocationRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Location
        public ActionResult Index()
        {
            return View(LocationRepo.ListAllLocation());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,RackPosition,Status,Spare1,Spare2,Spare3,Spare4,Spare5")] Location location)
        {
            db.Locations.Add(location);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}