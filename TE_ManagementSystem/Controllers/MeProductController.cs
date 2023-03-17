using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;
using System.Text.Json;

namespace TE_ManagementSystem.Controllers
{
    public class MeProductController : Controller
    {
        IMeProductRepo MeProductRepo = new MeProductRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: MeProduct
        public ActionResult Index()
        {
            return View(MeProductRepo.ListAllMeProduct());
        }

        [HttpGet]
        public ActionResult Create()
        {            
            int maxId = db.MeProducts.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
            maxId += 1;
            ViewBag.SuggestedNewMeProdId = maxId;
            ViewBag.Suppliers = db.Suppliers;
            ViewBag.Kind = db.Kinds;
            ViewBag.Customer = db.Customers;
            ViewBag.Kpn = db.KPNs;
            ViewBag.Opid = db.Employees;
            ViewBag.ProcessKind = db.KindProcesses;

            var kindData = db.Kinds;
            var supplierData = db.Suppliers;
            var customerData = db.Customers;
            var kpnData = db.KPNs;
            var opidData = db.Employees;
            var processKindData = db.KindProcesses;

            List<SelectListItem> selectKindListItems = new List<SelectListItem>();
            List<SelectListItem> selectSupplierListItems = new List<SelectListItem>();
            List<SelectListItem> selectCustomerListItems = new List<SelectListItem>();
            List<SelectListItem> selectKpnListItems = new List<SelectListItem>();
            List<SelectListItem> selectOpidListItems = new List<SelectListItem>();
            List<SelectListItem> selectProcessKindListItems = new List<SelectListItem>();

            foreach (var item in kindData)
            {
                selectKindListItems.Add(new SelectListItem()
                {
                    Text = item.ID + "/" + item.Name,
                    Value = item.ID.ToString(),
                    Selected = false
                });
            }

            foreach (var item in supplierData)
            {
                selectSupplierListItems.Add(new SelectListItem()
                {
                    Text = item.ID + "/" + item.Name,
                    Value = item.ID.ToString(),
                    Selected = false
                });
            }

            foreach (var item in customerData)
            {
                selectCustomerListItems.Add(new SelectListItem()
                {
                    Text = item.ID + "/" + item.CustCode + "/" + item.Name,
                    Value = item.ID.ToString(),
                    Selected = false
                });
            }

            foreach (var item in kpnData)
            {
                selectKpnListItems.Add(new SelectListItem()
                {
                    Text = item.ID + "/" + item.Name,
                    Value = item.ID.ToString(),
                    Selected = false
                });
            }

            foreach (var item in opidData)
            {
                selectOpidListItems.Add(new SelectListItem()
                {
                    Text = item.Opid + "/" + item.Name,
                    Value = item.Opid.ToString(),
                    Selected = false
                });
            }

            foreach (var item in processKindData)
            {
                selectProcessKindListItems.Add(new SelectListItem()
                {
                    Text = item.ID + "/" + item.Name,
                    Value = item.ID.ToString(),
                    Selected = false
                });
            }

            ViewBag.listKind = selectKindListItems;
            ViewBag.listSupplier = selectSupplierListItems;
            ViewBag.listCustomer = selectCustomerListItems;
            ViewBag.listKPN = selectKpnListItems;
            ViewBag.listOpid = selectOpidListItems;
            ViewBag.listProcessKind = selectProcessKindListItems;

            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,ProdName,KindID,CustomerID,SupplierID,Quantity,ShiftTime,Pb,Image,ComList,Spare1,Spare2,Spare3,Spare4,Spare5")] MeProduct meProduct, FormCollection form)
        //{
        //    int maxId = db.MeProduct.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
        //    maxId += 1;
        //    meProduct.ID = maxId;
        //    meProduct.IsStock = false;
        //    //string tmp = form["ComList"].ToString();
        //    meProduct.ComList = Request["ComList"].ToString();
        //    db.MeProduct.Add(meProduct);
            
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProdName,KindID,KindProcessID,CustomerID,SupplierID,Opid,Quantity,ShiftTime,Pb,Image,ComList,Spare1,Spare2,Spare3,Spare4,Spare5,Test")] MeProduct meProduct)
        {
            int maxId = db.MeProducts.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
            maxId += 1;
            meProduct.ID = maxId;
            meProduct.IsStock = false;
            meProduct.IsReturnMe = false;
            List<Mutiplekpn> mutiplekpns = JsonSerializer.Deserialize<List<Mutiplekpn>>(meProduct.Test);
            foreach (var item in mutiplekpns)
            {
                meProduct.ComList += item.value + ",";
            }

            //meProduct.ComList = JsonSerialize(meProduct.Test);
            if (meProduct.ShiftTime == 0)
            {
                meProduct.ShiftTime = 10;
            }

            db.MeProducts.Add(meProduct);

            LabelRule labelRule = new LabelRule();
            int maxlabelRuleId = db.LabelRules.DefaultIfEmpty().Max(r => r == null ? 0 : r.ID);
            maxlabelRuleId +=1;
            labelRule.ID = maxlabelRuleId;
            labelRule.KindID = meProduct.KindID;
            labelRule.ProcessKindID = meProduct.KindProcessID;

            var chkRepeat = db.LabelRules.Where
            (k => (k.KindID == labelRule.KindID && k.ProcessKindID == labelRule.ProcessKindID)).FirstOrDefault();

            if (chkRepeat is null)
            {
                var kindProcessess = db.KindProcesses.Where
                (k => k.ID == labelRule.ProcessKindID).FirstOrDefault();

                var kinds = db.Kinds.Where
                (k => k.ID == labelRule.KindID).FirstOrDefault();

                labelRule.LabelRule1 = kindProcessess.Number + kinds.Number + "00000";
                db.LabelRules.Add(labelRule);
            }
            else
            {
                //已有此規則不需建立新的
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Vue()
        {
            return View();
        }


        public class Mutiplekpn
        {
            public string text { get; set; }
            public string value { get; set; }
        }

    }
}