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
        [Authorize(Users = "1,2,3,4")]
        public ActionResult Index()
        {
            return View(LocationRepo.ListAllLocation());
        }

        [Authorize(Users = "1,2")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Users = "1,2")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,RackPosition,Status,Spare1,Spare2,Spare3,Spare4,Spare5")] Location location)
        {
            try
            {
                if (this.CheckInputErr(location)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }

                int maxId = db.Locations.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                maxId += 1;
                location.ID = maxId;
                location.Status = true;
                db.Locations.Add(location);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" + ex });
            }
        }

        private bool CheckInputErr(Location location)
        {
            if (location.Name == null || location.Name == string.Empty) { return true; };
            if (location.RackPosition == null || location.RackPosition == string.Empty) { return true; };

            return false;
        }

    }
}