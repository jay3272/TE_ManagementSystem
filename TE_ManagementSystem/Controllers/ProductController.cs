using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;
using System.Net;

namespace TE_ManagementSystem.Controllers
{
    public class ProductController : Controller
    {
        IProductRepo ProductRepo = new ProductRepo();
        IMeProductRepo MeProductRepo = new MeProductRepo();
        ILabelRuleRepo LabelRuleRepo = new LabelRuleRepo();
        IPORepo PORepo = new PORepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Product
        [Authorize(Users = "1,2,3,4")]
        public ActionResult Index()
        {
            return View(ProductRepo.ListAllProductUpdateDue());
        }

        // GET:Resume
        [Authorize(Users = "1,2,3,4")]
        public ActionResult Resume(string id)
        {
            if (id == string.Empty)
            {

            }
            var pohistory = PORepo.GetProductTransactionById(id);
            ViewBag.PoHistory = pohistory;

            return View(ProductRepo.LinkToResume(id));


        }

        [HttpGet]
        [Authorize(Users = "1,2")]
        public ActionResult Create()
        {
            var readyImportProduct = MeProductRepo.ListAllMeProductNotStock();

            int maxId = db.Products.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
            maxId += 1;
            ViewBag.SuggestedNewProdId = maxId;
            ViewBag.MeProducts = readyImportProduct;
            ViewBag.Locations = db.Locations;


            var locationData = db.Locations.Select(x => x.Name).Distinct();
            var rackData = db.Locations.Select(x => x.RackPosition).Distinct();
            var meProductData = readyImportProduct;

            List<SelectListItem> selectLocationListItems = new List<SelectListItem>();
            List<SelectListItem> selectRackListItems = new List<SelectListItem>();
            List<SelectListItem> selectMeProductListItems = new List<SelectListItem>();

            foreach (var item in locationData)
            {
                selectLocationListItems.Add(new SelectListItem()
                {
                    Text = item,
                    Selected = false
                });
            }

            foreach (var item in rackData)
            {
                selectRackListItems.Add(new SelectListItem()
                {
                    Text = item,
                    Selected = false
                });
            }

            foreach (var item in meProductData)
            {
                selectMeProductListItems.Add(new SelectListItem()
                {
                    Text = item.ID + "/" + item.ProdName + "/" + item.ComList,
                    Value = item.ID.ToString(),
                    Selected = false
                });
            }

            ViewBag.listLocation = selectLocationListItems;
            ViewBag.listRack = selectRackListItems;
            ViewBag.listMeProduct = selectMeProductListItems;


            return View();
        }

        [HttpPost]
        [Authorize(Users = "1,2")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NumberID,RFID,Status,LocationID,Room,Rack,EngID,StockDate,Life,LastBorrowDate,LastReturnDate,UseLastDate,Usable,Overdue,Spare1,Spare2,Spare3,Spare4,Spare5")] Product Product)
        {
            try
            {
                if (this.CheckInputErr(Product)) { return Json(new { ReturnStatus = "error" }); }

                const int cNumberSeries = 5;
                string labelRuleNumber = LabelRuleRepo.GetLabelRule(Product.EngID);
                Product.LocationID = LabelRuleRepo.GetLocationID(Product.Room, Product.Rack);
                Product.Usable = true;

                //取label規則+1補0
                string labelHeader = labelRuleNumber.Substring(0, 3);
                string slabelNumber;
                int labelNumber;
                int.TryParse(labelRuleNumber.Substring(3, cNumberSeries), out labelNumber);
                labelNumber += 1;
                slabelNumber = labelNumber.ToString().PadLeft(cNumberSeries, '0');
                Product.NumberID = labelHeader + slabelNumber;
                //回填DB
                if (LabelRuleRepo.UpdateLabelRuleNumber(Product.EngID, Product.NumberID))
                {

                    int maxId = db.Products.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                    maxId += 1;
                    Product.ID = maxId;
                    Product.StockDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

                    if (Product.Usable)
                    {
                        Product.Status = "倉庫";
                    }
                    else
                    {
                        Product.Status = "借出";
                    }

                    db.Products.Add(Product);

                    db.SaveChanges();

                    if (MeProductRepo.UpdateMeProductStock(Product.EngID))
                    {

                    }
                    else
                    {
                        //更新失敗
                        return Json(new { ReturnStatus = "error" });
                    }

                }
                else
                {
                    //更新失敗
                    return Json(new { ReturnStatus = "error" });
                }


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error" });
            }
        }

        //GET: Product/Edit
        [Authorize(Users = "1")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);

        }

        public ActionResult DisplayingImage(int imgid)
        {
            ManagementContextEntities db = new ManagementContextEntities();
            var img = db.MeProducts.SingleOrDefault(x => x.ID == imgid);
            byte[] imageBuff = { 136, 12 };
            if (!(img.ImageByte is null))
            {
                ImageViewModel imageViewModel = new ImageViewModel();
                imageBuff = imageViewModel.CreateThumbnailImage(300, 300, img.ImageByte, true);
            }

            return File(imageBuff, "image/png");
        }

        private bool CheckInputErr(Product Product)
        {
            if (Product.NumberID == null || Product.NumberID == string.Empty) { return true; };
            if (Product.Room == null || Product.Room == string.Empty) { return true; };
            if (Product.Rack == null || Product.Rack == string.Empty) { return true; };

            return false;
        }

    }
}