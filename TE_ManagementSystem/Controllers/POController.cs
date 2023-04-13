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
        public ActionResult Index()
        {
            return View(PORepo.ListAllProductTransaction());
        }

        // GET: PO/Create
        public ActionResult Create(bool IsReturn)
        {
            int maxId = db.ProductTransactions.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
            maxId += 1;
            ViewBag.SuggestedNewPoId = maxId;
            ViewBag.MeProducts = db.MeProducts;
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


            var productData = db.Products;

            List<SelectListItem> selectProductListItems = new List<SelectListItem>();

            foreach (var item in productData)
            {
                selectProductListItems.Add(new SelectListItem()
                {
                    Text = item.MeProduct.ComList + "/" + item.MeProduct.ProdName + "/" + item.NumberID,
                    Value = item.NumberID,
                    Selected = false
                });
            }

            ViewBag.listProduct = selectProductListItems;


            return View();
        }

        [HttpPost]
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

    }
}