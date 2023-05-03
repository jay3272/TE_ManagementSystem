using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models.Repo;
using TE_ManagementSystem.Models;
using System.Net;
using System.Data.Entity;

namespace TE_ManagementSystem.Controllers
{
    [Authorize]
    public class POController : Controller
    {
        IPORepo PORepo = new PORepo();

        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: PO
        [Authorize(Users = "1,2,3,4,5")]
        public ActionResult Index()
        {
            return View(PORepo.ListAllProductTransaction());
        }

        // GET: PO/Create
        [Authorize(Users = "1,2,3,4")]
        public ActionResult Create(bool IsReturn)
        {
            try
            {
                int maxId = db.ProductTransactions.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                maxId += 1;
                ViewBag.SuggestedNewPoId = maxId;
                //ViewBag.MeProducts = db.MeProducts;
                ViewBag.IsReturn = IsReturn;

                if (IsReturn)
                {
                    ViewBag.PoTitle = "歸還頁面";
                    ViewBag.PoBtn = "歸還";
                }
                else
                {
                    ViewBag.PoTitle = "借出頁面";
                    ViewBag.PoBtn = "借出";
                }


                IProductRepo ProductRepo = new ProductRepo();
                var productData = ProductRepo.ListAllProductInStock();

                List<SelectListItem> selectProductListItems = new List<SelectListItem>();

                foreach (var item in productData)
                {
                    selectProductListItems.Add(new SelectListItem()
                    {
                        Text = item.MeProduct.ProdName + "/" + item.NumberID,
                        Value = item.NumberID,
                        Selected = false
                    });
                }

                ViewBag.Products = productData;
                ViewBag.listProduct = selectProductListItems;


                return View();
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" + ex });
            }
        }

        [HttpPost]
        [Authorize(Users = "1,2,3,4")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Opid,ProductID,IsReturn,IsToFix,BorrowDay,RegisterDate")] ProductTransaction productTransaction, bool IsReturn)
        {
            try
            {
                if (this.CheckInputErr(productTransaction)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }

                int maxId = db.ProductTransactions.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                maxId += 1;
                productTransaction.ID = maxId;
                productTransaction.IsReturn = IsReturn;
                productTransaction.RegisterDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                db.ProductTransactions.Add(productTransaction);

                var products = db.Products.Where
                (m => m.NumberID == productTransaction.ProductID).FirstOrDefault();

                productTransaction.BorrowDay = products.MeProduct.ShiftTime;

                // 更新
                if (IsReturn)
                {
                    if (!products.Usable)
                    {
                        products.Status = "倉庫";
                        products.LastReturnDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        products.Usable = true;
                    }
                    else
                    {
                        //TempData["message"] = products.NumberID + ",已在儲室!";
                        return Json(new { ReturnStatus = "error", ReturnData = products.NumberID + ",已在儲室!" });
                    }
                }
                else
                {
                    if (products.Usable)
                    {
                        products.Status = "借出";
                        products.LastBorrowDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        products.Usable = false;
                    }
                    else
                    {
                        TempData["message"] = products.NumberID + ",已借出!";
                        return RedirectToAction("Index");
                    }

                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" + ex });
            }
        }

        //GET: PO/Edit
        public ActionResult Edit(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ProductTransaction productTransaction = db.ProductTransactions.Find(id);

                if (productTransaction == null)
                {
                    return HttpNotFound();
                }
                return View(productTransaction);
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "Edit(), ex:" + ex });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Opid,ProductID,CustomerID,Kpn,BorrowReturn,BorrowDay,RegisterDate,Overdue")] ProductTransaction productTransaction)
        {
            try
            {

                var productTransactions = db.ProductTransactions.Where
                    (m => m.ID == productTransaction.ID).FirstOrDefault();

                productTransactions.Product.MeProduct.ComList = productTransaction.Product.MeProduct.ComList;
                db.SaveChanges();
                return RedirectToAction("Index");

                //if (ModelState.IsValid)
                //{
                //    db.Entry(productTransaction).State = EntityState.Modified;
                //    db.SaveChanges();
                //    return RedirectToAction("Index");
                //}
                //return View(productTransaction);
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "Edit(), ex:" + ex });
            }
        }

        public ActionResult DisplayingImage(string imgid)
        {
            try
            {
                ManagementContextEntities db = new ManagementContextEntities();
                var img = db.Products.SingleOrDefault(p => p.NumberID == imgid.Trim());
                byte[] imageBuff = { 136, 12 };
                if (!(img.MeProduct.ImageByte is null))
                {
                    ImageViewModel imageViewModel = new ImageViewModel();
                    imageBuff = imageViewModel.CreateThumbnailImage(50, 50, img.MeProduct.ImageByte, true);
                }

                return File(imageBuff, "image/png");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "DisplayingImage(), ex:" + ex });
            }
        }

        public ActionResult ViewImage(string text)
        {
            try
            {
                byte[] imageBuff = { 136, 12 };
                if (text == "")
                {
                    //跳出錯誤訊息
                }
                else
                {
                    var img = db.Products.SingleOrDefault(x => x.NumberID == text.Trim());
                    if (!(img.MeProduct.ImageByte is null))
                    {
                        ImageViewModel imageViewModel = new ImageViewModel();
                        imageBuff = imageViewModel.CreateThumbnailImage(200, 200, img.MeProduct.ImageByte, true);
                    }
                    else
                    {
                        return Json(new { ReturnStatus = "error", ReturnData = "請確認有選擇圖片 !" });
                    }
                }

                return Json(new { base64imgage = Convert.ToBase64String(imageBuff) }
              , JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "ViewImage(), ex:" + ex });
            }
        }

        private bool CheckInputErr(ProductTransaction productTransaction)
        {
            if (productTransaction.ProductID == null || productTransaction.ProductID == string.Empty) { return true; };
            if (productTransaction.Opid == null) { return true; };

            return false;
        }

    }
}