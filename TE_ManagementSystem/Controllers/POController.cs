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
    public class POController : Controller
    {
        IPORepo PORepo = new PORepo();

        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: PO
        [Authorize(Users = "1,2,3,4")]
        public ActionResult Index()
        {
            return View(PORepo.ListAllProductTransaction());
        }

        // GET: PO/Create
        [Authorize(Users = "1,2,3,4")]
        public ActionResult Create(bool IsReturn)
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

        [HttpPost]
        [Authorize(Users = "1,2,3,4")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Opid,ProductID,IsReturn,IsToFix,BorrowDay,RegisterDate")] ProductTransaction productTransaction, bool IsReturn)
        {
            int maxId = db.ProductTransactions.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
            maxId += 1;
            productTransaction.ID = maxId;
            productTransaction.IsReturn = IsReturn;
            productTransaction.RegisterDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
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
                    products.LastReturnDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    products.Usable = true;
                }
                else
                {
                    TempData["message"] = products.NumberID + ",已在儲室!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (products.Usable)
                {
                    products.Status = "借出";
                    products.LastBorrowDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
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

        //GET: PO/Edit
        public ActionResult Edit(int id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Opid,ProductID,CustomerID,Kpn,BorrowReturn,BorrowDay,RegisterDate,Overdue")] ProductTransaction productTransaction)
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

        public ActionResult DisplayingImage(string imgid)
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

        public ActionResult ViewImage(string text)
        {
            byte[] imageBuff = { 136, 12 };
            if (text == "")
            {
                //跳出錯誤訊息
            }
            var img = db.Products.SingleOrDefault(x => x.NumberID == text.Trim());
            if (!(img.MeProduct.ImageByte is null))
            {
                ImageViewModel imageViewModel = new ImageViewModel();
                imageBuff = imageViewModel.CreateThumbnailImage(200, 200, img.MeProduct.ImageByte, true);
            }

            return Json(new { base64imgage = Convert.ToBase64String(imageBuff) }
          , JsonRequestBehavior.AllowGet);
        }

    }
}