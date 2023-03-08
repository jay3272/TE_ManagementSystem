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

        // GET: KindProcess
        public ActionResult Index()
        {
            return View(KindProcessRepo.ListAllKindProcess());
        }
    }
}