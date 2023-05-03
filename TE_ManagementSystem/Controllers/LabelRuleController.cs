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
    public class LabelRuleController : Controller
    {
        ILabelRuleRepo LabelRuleRepo = new LabelRuleRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: LabelRule
        [Authorize(Users = "1,2,3,4,5")]
        public ActionResult Index()
        {
            return View(LabelRuleRepo.ListAllLabelRule());
        }

        //[HttpGet]
        //public ActionResult Create()
        //{
        //    int maxId = db.LabelRules.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
        //    maxId += 1;
        //    ViewBag.SuggestedLabelRuleId = maxId;
        //    ViewBag.Kind = db.Kinds;
        //    ViewBag.KindProcess = db.KindProcesses;

        //    var kindData = db.Kinds;
        //    var kindProcessData = db.KindProcesses;

        //    List<SelectListItem> selectKindListItems = new List<SelectListItem>();
        //    List<SelectListItem> selectKindProcessListItems = new List<SelectListItem>();

        //    foreach (var item in kindData)
        //    {
        //        selectKindListItems.Add(new SelectListItem()
        //        {
        //            Text = item.ID + "/" + item.Name,
        //            Value = item.ID.ToString(),
        //            Selected = false
        //        });
        //    }

        //    foreach (var item in kindProcessData)
        //    {
        //        selectKindProcessListItems.Add(new SelectListItem()
        //        {
        //            Text = item.ID + "/" + item.Name,
        //            Value = item.ID.ToString(),
        //            Selected = false
        //        });
        //    }

        //    ViewBag.listKind = selectKindListItems;
        //    ViewBag.listKindProcess = selectKindProcessListItems;

        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,ProcessKindID,KindID,LabelRule")] LabelRule labelRule)
        //{
        //    int maxId = db.LabelRules.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
        //    maxId += 1;
        //    labelRule.ID = maxId;
        //    //var chkRepeat = db.LabelRule.Where(x => x.KindID == labelRule.KindID || x.ProcessKindID == labelRule.ProcessKindID).FirstOrDefault();

        //    //List<KindProcess> kindProcessesList = new List<KindProcess>();
        //    //List<Kind> kindList = new List<Kind>();

        //    //using (ProductContext db = new ProductContext())
        //    //{
        //    //    kindProcessesList = (from k in db.KindProcess where k.ID == labelRule.ProcessKindID select k).ToList();
        //    //    kindList = (from k in db.Kind where k.ID == labelRule.KindID select k).ToList();

        //    //}
        //    var chkRepeat = db.LabelRules.Where
        //    (k => (k.KindID == labelRule.KindID && k.ProcessKindID == labelRule.ProcessKindID)).FirstOrDefault();

        //    var kindProcessess = db.KindProcesses.Where
        //    (k => k.ID == labelRule.ProcessKindID).FirstOrDefault();

        //    var kinds = db.Kinds.Where
        //    (k => k.ID == labelRule.KindID).FirstOrDefault();

        //    if (chkRepeat is null)
        //    {
        //        labelRule.LabelRule1 = kindProcessess.Number + kinds.Number + "00000";
        //        db.LabelRules.Add(labelRule);

        //        db.SaveChanges();
        //    }
        //    else
        //    {
        //        TempData["message"] = kindProcessess.Number + kinds.Number + "00000" + ",標籤規則已產生過!";
        //        return RedirectToAction("Index");
        //    }

        //    return RedirectToAction("Index");
        //}

    }
}