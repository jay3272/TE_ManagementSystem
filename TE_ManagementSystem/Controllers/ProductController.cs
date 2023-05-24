using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;
using System.Net;
using System.Reflection;
using PagedList;
using PagedList.Mvc;
using System.Linq.Dynamic;
using Newtonsoft.Json;

namespace TE_ManagementSystem.Controllers
{
    [Authorize]
    public class ProductController : ProjectBase
    {
        IProductRepo ProductRepo = new ProductRepo();
        IMeProductRepo MeProductRepo = new MeProductRepo();
        ILabelRuleRepo LabelRuleRepo = new LabelRuleRepo();
        IPORepo PORepo = new PORepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Product
        [Authorize(Users = "1,2,3,4,5")]
        public ActionResult Index()
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);
            //return View(ProductRepo.ListAllProductUpdateDue());
            //List<Product> products = db.Products.ToList();
            //return View(db.Products.Where(x => x.NumberID.StartsWith(search) || search==null).ToList().ToPagedList(i ?? 1, 10));
            return View();
        }

        // GET:Resume
        [Authorize(Users = "1,2,3,4")]
        public ActionResult Resume(string id)
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);

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
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);

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
        public ActionResult Create([Bind(Include = "ID,NumberID,RFID,Status,LocationID,Room,Rack,EngID,StockDate,Life,LastBorrowDate,LastReturnDate,UseLastDate,Usable,Overdue,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateEmployee")] Product Product)
        {
            try
            {
                if (this.CheckInputErr(Product)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }
                Product.EngID = MeProductRepo.GetMeProductIdByName(Product.Spare5);

                const int cNumberSeries = 5;
                string labelRuleNumber;
                //判斷待處理治具
                MeProduct meProduct = new MeProduct();
                var meProd = db.MeProducts.Where
                    (m => m.ID == Product.EngID).FirstOrDefault();

                if (meProd.KindID == 0 || meProd.KindProcessID == 0)
                {
                    //labelRuleNumber = LabelRuleRepo.GetDefaultLabelRule();
                    return Json(new { ReturnStatus = "error", ReturnData = "種類與製程種類未分類不允許匯入 !" });
                }
                else
                {
                    this.LabelRuleRepo.GenerateLabelRule(Product.EngID); 
                    labelRuleNumber = LabelRuleRepo.GetLabelRule(Product.EngID);
                    Product.LocationID = LabelRuleRepo.GetLocationID(Product.Room, Product.Rack);
                    Product.Usable = true;
                }

                //取label規則+1補0
                string labelHeader = labelRuleNumber.Substring(0, 3);
                string slabelNumber;
                int labelNumber;
                int.TryParse(labelRuleNumber.Substring(3, cNumberSeries), out labelNumber);
                if (labelNumber < 99999)
                {
                    labelNumber += 1;
                    slabelNumber = labelNumber.ToString().PadLeft(cNumberSeries, '0');
                    Product.NumberID = labelHeader + slabelNumber;
                }
                else
                {
                    return Json(new { ReturnStatus = "error", ReturnData = "編碼超過99999 !" });
                }

                //回填DB
                if (LabelRuleRepo.UpdateLabelRuleNumber(Product.EngID, Product.NumberID))
                {

                    int maxId = db.Products.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                    maxId += 1;
                    Product.ID = maxId;
                    Product.UpdateEmployee = Session["UsrName"].ToString();

                    Product.StockDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    if (Product.Usable)
                    {
                        Product.Status = "倉庫";
                    }
                    else
                    {
                        Product.Status = "借出";
                    }

                    db.Products.Add(Product);

                    if (MeProductRepo.UpdateMeProductStock(Product.EngID))
                    {

                    }
                    else
                    {
                        //更新失敗
                        return Json(new { ReturnStatus = "error", ReturnData = "更新到倉庫狀態異常 !" });
                    }

                    db.SaveChanges();

                }
                else
                {
                    //更新失敗
                    return Json(new { ReturnStatus = "error", ReturnData = "更新治具編碼規則異常 !" });
                }

                this.logUtil.AppendMethod("Save Create");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" + ex });
            }
        }

        //GET: Product/Edit
        [Authorize(Users = "1,2")]
        public ActionResult Edit(string id)
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);

            try
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
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "Edit(), ex:" + ex });
            }
        }

        [HttpPost]
        [Authorize(Users = "1,2")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NumberID,RFID,Status,LocationID,Room,Rack,EngID,StockDate,Life,LastBorrowDate,LastReturnDate,UseLastDate,Usable,Overdue,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateDate,UpdateEmployee")] Product Product)
        {
            try
            {
                if (this.CheckInputErr(Product)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }

                Product.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Product.UpdateEmployee = Session["UsrName"].ToString();

                db.SaveChanges();
                this.logUtil.AppendMethod("Save Update");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" + ex });
            }
        }

        public ActionResult DisplayingImage(int imgid)
        {
            try
            {
                ManagementContextEntities db = new ManagementContextEntities();
                var img = db.MeProducts.SingleOrDefault(x => x.ID == imgid);
                byte[] imageBuff = { 136, 12 };
                if (!(img.ImageByte is null))
                {
                    ImageViewModel imageViewModel = new ImageViewModel();
                    imageBuff = imageViewModel.CreateThumbnailImage(600, 400, img.ImageByte, true);
                }

                return File(imageBuff, "image/png");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "DisplayingImage(), ex:" + ex });
            }
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

            List<Product> productsList = new List<Product>();
            productsList = ProductRepo.ListAllProductUpdateDue().ToList<Product>();
            int totalrows = productsList.Count;
            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                productsList = productsList
                    .Where(x => x.NumberID.ToLower().Contains(searchValue.ToLower()) || x.RFID.ToLower().Contains(searchValue.ToLower())).ToList<Product>();
            }
            int totalrowsafterfiltering = productsList.Count;
            //sorting
            productsList = productsList.OrderBy(sortColumnName + " " + sortDirection).ToList<Product>();
            //paging
            productsList = productsList.Skip(start).Take(length).ToList<Product>();

            return Json(new { data = productsList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        private bool CheckInputErr(Product Product)
        {
            if (Product.Spare5 == null || Product.Spare5 == string.Empty) { return true; };
            if (Product.Room == null || Product.Room == string.Empty) { return true; };
            if (Product.Rack == null || Product.Rack == string.Empty) { return true; };

            return false;
        }

    }
}