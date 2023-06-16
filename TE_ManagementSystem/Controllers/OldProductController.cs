using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models.Repo;
using TE_ManagementSystem.Models;
using System.Linq.Dynamic;

namespace TE_ManagementSystem.Controllers
{
    public class OldProductController : Controller
    {
        IOldProductRepo OldProductRepo = new OldProductRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: OldProduct
        [Authorize(Users = "1,2,3,4,5")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetList()
        {
            //server side parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<ViewOldProduct> oldproductsList = new List<ViewOldProduct>();
            oldproductsList = OldProductRepo.ListAllOldProduct().ToList<ViewOldProduct>();
            int totalrows = oldproductsList.Count;
            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                oldproductsList = oldproductsList
                    .Where(x => x.ProdName.ToLower().Contains(searchValue.ToLower())).ToList<ViewOldProduct>();
            }
            int totalrowsafterfiltering = oldproductsList.Count;
            //sorting
            oldproductsList = oldproductsList.OrderBy(sortColumnName + " " + sortDirection).ToList<ViewOldProduct>();
            //paging
            oldproductsList = oldproductsList.Skip(start).Take(length).ToList<ViewOldProduct>();

            return Json(new { data = oldproductsList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

    }
}