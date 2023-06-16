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
using System.Xml.Linq;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using static TE_ManagementSystem.Controllers.MeProductController;

namespace TE_ManagementSystem.Controllers
{
    [Authorize]
    public class ProductController : ProjectBase
    {
        IProductRepo ProductRepo = new ProductRepo();
        IMeProductRepo MeProductRepo = new MeProductRepo();
        ILabelRuleRepo LabelRuleRepo = new LabelRuleRepo();
        IPORepo PORepo = new PORepo();
        ISupplierRepo SupplierRepo = new SupplierRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Product
        [Authorize(Users = "1,2,3,4,5")]
        public ActionResult Index()
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);
            GlobalValue.viewproductsList = ProductRepo.ListAllProductUpdateDue().ToList<ViewProduct>();
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

            GlobalValue.LoginUserName = Convert.ToString(Session["UsrName"] ?? "").Trim();

            if (GlobalValue.LoginUserName.ToString().Count() > 0)
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
            else
            {
                return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [Authorize(Users = "1,2")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NumberID,RFID,Status,LocationID,Room,Rack,EngID,StockDate,Life,LastBorrowDate,LastReturnDate,UseLastDate,Usable,Overdue,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateEmployee")] Product Product)
        {
            try
            {
                if (this.CheckInputErr(Product)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }
                Product.EngID = MeProductRepo.GetMeProductIdByName(Product.Spare5.Trim());

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
                    Product.LocationID = LabelRuleRepo.GetLocationID(Product.Room.Trim(), Product.Rack.Trim());
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

                    try
                    {
                        if (Session["UsrName"].ToString().Count() > 0)
                        {
                            Product.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            Product.UpdateEmployee = Convert.ToString(Session["UsrName"] ?? "").Trim();
                        }
                        else
                        {
                            return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" });
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" });
                    }

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
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" });
            }
        }

        //[HttpGet]
        //[Authorize(Users = "1,2")]
        //public ActionResult OldCreate()
        //{
        //    this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);

        //    this.loaddefault();

        //    GlobalValue.LoginUserName = Convert.ToString(Session["UsrName"] ?? "").Trim();

        //    if (GlobalValue.LoginUserName.ToString().Count() > 0)
        //    {
        //        int maxId = db.Products.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
        //        maxId += 1;
        //        ViewBag.SuggestedNewProdId = maxId;
        //        ViewBag.Locations = db.Locations;


        //        var locationData = db.Locations.Select(x => x.Name).Distinct();
        //        var rackData = db.Locations.Select(x => x.RackPosition).Distinct();

        //        List<SelectListItem> selectLocationListItems = new List<SelectListItem>();
        //        List<SelectListItem> selectRackListItems = new List<SelectListItem>();

        //        foreach (var item in locationData)
        //        {
        //            selectLocationListItems.Add(new SelectListItem()
        //            {
        //                Text = item,
        //                Selected = false
        //            });
        //        }

        //        foreach (var item in rackData)
        //        {
        //            selectRackListItems.Add(new SelectListItem()
        //            {
        //                Text = item,
        //                Selected = false
        //            });
        //        }

        //        ViewBag.listLocation = selectLocationListItems;
        //        ViewBag.listRack = selectRackListItems;

        //        return View();
        //    }
        //    else
        //    {
        //        return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" }, JsonRequestBehavior.AllowGet);
        //    }

        //}

        //[HttpPost]
        //[Authorize(Users = "1,2")]
        //[ValidateAntiForgeryToken]
        //public ActionResult OldCreate([Bind(Include = "ID,NumberID,RFID,Status,LocationID,Room,Rack,EngID,StockDate,Life,LastBorrowDate,LastReturnDate,UseLastDate,Usable,Overdue,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateEmployee,OldQuantity,OldSupplier,OldState,OldKpn,OldAppendix,OldKindId")] Product Product)
        //{

        //    int maxEngId = db.MeProducts.DefaultIfEmpty().Max(m => m == null ? 0 : m.ID) + 1;
        //    int maxSupId = db.Suppliers.DefaultIfEmpty().Max(s => s == null ? 0 : s.ID) + 1;

        //    try
        //    {
        //        if (this.CheckOldInputErr(Product)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }

        //        try
        //        {
        //            if (Session["UsrName"].ToString().Count() > 0)
        //            {
        //                Product.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //                Product.UpdateEmployee = Convert.ToString(Session["UsrName"] ?? "").Trim();
        //            }
        //            else
        //            {
        //                return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" });
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" });
        //        }


        //        if (Product.NumberID.Trim().Length > 3 && Product.NumberID.Trim().Length < 8)
        //        {
        //            //add MeProduct
        //            MeProduct meproduct = new MeProduct();
        //            try
        //            {
        //                meproduct.ID = maxEngId;
        //                meproduct.ProdName = Product.Spare5.Trim();
        //                meproduct.KindID = (int)Product.OldKindId;
        //                meproduct.KindProcessID = 0;
        //                meproduct.CustomerID = 0;
        //                var tmpSupplier = db.Suppliers.Where(s => s.Name == Product.OldSupplier.Trim()).FirstOrDefault();

        //                try
        //                {
        //                    if (tmpSupplier == null)
        //                    {
        //                        Supplier supplier = new Supplier();
        //                        supplier.ID = maxSupId;
        //                        supplier.Name = Product.OldSupplier.Trim();
        //                        supplier.Email = "NA";
        //                        supplier.Phone = "NA";
        //                        supplier.Address = "NA";
        //                        db.Suppliers.Add(supplier);
        //                        db.SaveChanges();
        //                        tmpSupplier = db.Suppliers.Where(s => s.Name == Product.OldSupplier.Trim()).FirstOrDefault();
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    if (SupplierRepo.DeleteSupplier(maxSupId, Product.OldSupplier.Trim()))
        //                    {
        //                        return Json(new { ReturnStatus = "error", ReturnData = "新增供應商異常 !" });
        //                    }
        //                    else
        //                    {
        //                        return Json(new { ReturnStatus = "error", ReturnData = "新增供應商異常，回朔刪除異常，請通知工程師 !" });
        //                    }
        //                }

        //                meproduct.SupplierID = tmpSupplier.ID;
        //                meproduct.Opid = Session["UsrOpid"].ToString();
        //                meproduct.Quantity = (int)Product.OldQuantity;
        //                meproduct.ShiftTime = 10;
        //                meproduct.IsStock = true;
        //                meproduct.IsReturnMe = false;
        //                meproduct.Pb = false;
        //                meproduct.ComList = Product.OldKpn.Trim();
        //                meproduct.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //                meproduct.UpdateEmployee = Convert.ToString(Session["UsrName"] ?? "").Trim();

        //                db.MeProducts.Add(meproduct);
        //                db.SaveChanges();

        //            }
        //            catch (Exception ex)
        //            {
        //                if (SupplierRepo.DeleteSupplier(maxSupId, Product.OldSupplier.Trim()) && MeProductRepo.DeleteMeProduct(maxEngId, Product.Spare5.Trim()))
        //                {
        //                    return Json(new { ReturnStatus = "error", ReturnData = "新增供應商異常 !" });
        //                }
        //                else
        //                {
        //                    return Json(new { ReturnStatus = "error", ReturnData = "新增供應商異常，回朔刪除異常，請通知工程師 !" });
        //                }                        
        //            }
        //            //

        //            //新增OLD 治具
        //            int maxId = db.Products.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
        //            maxId += 1;
        //            Product.ID = maxId;
        //            Product.EngID = maxEngId;
        //            var tmpLocation = db.Locations.Where(l => (l.Name == Product.Room.Trim() && l.RackPosition == Product.Rack.Trim())).FirstOrDefault();
        //            if (tmpLocation == null)
        //            {
        //                Location location = new Location();
        //                location.ID = db.Locations.DefaultIfEmpty().Max(l => l == null ? 0 : l.ID) + 1;
        //                location.Name = Product.Room.Trim();
        //                location.RackPosition = Product.Rack.Trim();
        //                location.Status = true;
        //                db.Locations.Add(location);
        //                db.SaveChanges();
        //                tmpLocation = db.Locations.Where(l => (l.Name == Product.Room.Trim() && l.RackPosition == Product.Rack.Trim())).FirstOrDefault();
        //            }
        //            Product.LocationID = tmpLocation.ID;
        //            Product.Usable = true;

        //            if (Product.Usable)
        //            {
        //                Product.Status = "倉庫";
        //            }
        //            else
        //            {
        //                Product.Status = "借出";
        //            }

        //            Product.StockDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //            Product.Overdue = false;

        //            db.Products.Add(Product);
        //            db.SaveChanges();
        //            //
        //        }
        //        else
        //        {
        //            return Json(new { ReturnStatus = "error", ReturnData = "治具編號需3~7碼 !" });
        //        }


        //        this.logUtil.AppendMethod("Save Create");
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        if (SupplierRepo.DeleteSupplier(maxSupId, Product.OldSupplier.Trim()) && MeProductRepo.DeleteMeProduct(maxEngId, Product.Spare5.Trim()))
        //        {
        //            return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" });
        //        }
        //        else
        //        {
        //            return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複，回朔刪除異常，請通知工程師 !" });
        //        }                
        //    }
        //}

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
                var model = db.Products.Where(x => x.ID == Product.ID).FirstOrDefault();

                model.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                try
                {
                    if (Session["UsrName"].ToString().Count() > 0)
                    {
                        model.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        model.UpdateEmployee = Convert.ToString(Session["UsrName"] ?? "").Trim();
                    }
                    else
                    {
                        return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" });
                }

                db.SaveChanges();
                this.logUtil.AppendMethod("Save Update");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" });
            }
        }

        ////GET: Product/Edit
        //[Authorize(Users = "1,2")]
        //public ActionResult OldEdit(string id)
        //{
        //    this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);

        //    try
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }

        //        Product product = db.Products.Find(id);

        //        if (product == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(product);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { ReturnStatus = "error", ReturnData = "Edit(), ex:" + ex });
        //    }
        //}

        //[HttpPost]
        //[Authorize(Users = "1,2")]
        //[ValidateAntiForgeryToken]
        //public ActionResult OldEdit([Bind(Include = "ID,NumberID,RFID,Status,LocationID,Room,Rack,EngID,StockDate,Life,LastBorrowDate,LastReturnDate,UseLastDate,Usable,Overdue,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateDate,UpdateEmployee")] Product Product)
        //{
        //    try
        //    {
        //        if (this.CheckInputErr(Product)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }
        //        var model = db.Products.Where(x => x.ID == Product.ID).FirstOrDefault();

        //        model.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        //        try
        //        {
        //            if (Session["UsrName"].ToString().Count() > 0)
        //            {
        //                model.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //                model.UpdateEmployee = Convert.ToString(Session["UsrName"] ?? "").Trim();
        //            }
        //            else
        //            {
        //                return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" });
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" });
        //        }

        //        db.SaveChanges();
        //        this.logUtil.AppendMethod("Save Update");
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" });
        //    }
        //}

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
            int totalrowsafterfiltering;

            int totalrows = GlobalValue.viewproductsList.Count;
            List<ViewProduct> viewproductsPartialList = new List<ViewProduct>();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                switch (searchValue)
                {
                    case "逾期":
                        viewproductsPartialList = GlobalValue.viewproductsList
                            .Where(x => x.Status == "借出" && x.Overdue == true).ToList<ViewProduct>();
                        break;
                    case "期限內":
                        viewproductsPartialList = GlobalValue.viewproductsList
                            .Where(x => x.Status == "借出" && x.Overdue == false).ToList<ViewProduct>();
                        break;
                    case "未借出":
                        viewproductsPartialList = GlobalValue.viewproductsList
                            .Where(x => x.Status != "借出").ToList<ViewProduct>();
                        break;
                    default:
                        viewproductsPartialList = GlobalValue.viewproductsList
                            .Where(x => x.NumberID.ToLower().Contains(searchValue.ToLower()) || x.RFID.ToLower().Contains(searchValue.ToLower())
                             || x.Status.ToLower().Contains(searchValue.ToLower())).ToList<ViewProduct>();
                        break;
                }

                totalrowsafterfiltering = viewproductsPartialList.Count;
                //sorting
                viewproductsPartialList = viewproductsPartialList.OrderBy(sortColumnName + " " + sortDirection).ToList<ViewProduct>();
                //paging
                viewproductsPartialList = viewproductsPartialList.Skip(start).Take(length).ToList<ViewProduct>();

                return Json(new { data = viewproductsPartialList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

            }
            totalrowsafterfiltering = GlobalValue.viewproductsList.Count;
            //sorting
            viewproductsPartialList = GlobalValue.viewproductsList.OrderBy(sortColumnName + " " + sortDirection).ToList<ViewProduct>();
            //paging
            viewproductsPartialList = viewproductsPartialList.Skip(start).Take(length).ToList<ViewProduct>();

            return Json(new { data = viewproductsPartialList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        private void loaddefault()
        {
            ViewBag.Kind = db.Kinds;
            var kindData = db.Kinds;
            List<SelectListItem> selectKindListItems = new List<SelectListItem>();

            foreach (var item in kindData)
            {
                if (item.Name.Contains("舊分類"))
                {
                    selectKindListItems.Add(new SelectListItem()
                    {
                        Text = item.ID + "/" + item.Name,
                        Value = item.ID.ToString(),
                        Selected = false
                    });
                }
            }

            ViewBag.listKind = selectKindListItems;
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