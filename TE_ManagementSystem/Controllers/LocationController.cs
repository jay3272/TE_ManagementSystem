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

        // GET: Location
        public ActionResult Index()
        {
            return View(LocationRepo.ListAllLocation());
        }
    }
}