﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

namespace TE_ManagementSystem.Controllers
{
    [Authorize]
    public class MeProductController : Controller
    {
        IMeProductRepo MeProductRepo = new MeProductRepo();
        ILabelRuleRepo LabelRuleRepo = new LabelRuleRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: MeProduct
        [Authorize(Users = "1,2,3,4,5")]
        public ActionResult Index()
        {
            return View(MeProductRepo.ListAllMeProduct());
        }

        [Authorize(Users = "1,2,3")]
        public ActionResult Create()
        {
            this.loaddefault();
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
        [Authorize(Users = "1,2,3")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProdName,KindID,KindProcessID,CustomerID,SupplierID,Opid,Quantity,ShiftTime,Pb,Image,ComList,Spare1,Spare2,Spare3,Spare4,Spare5,Test,ImageByte,MeStockDate")] MeProduct meProduct)
        {
            try
            {
                if (this.CheckInputErr(meProduct)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }
                if (MeProductRepo.CheckNameRepeat(meProduct.ProdName)) { return Json(new { ReturnStatus = "error", ReturnData = "治具名稱重複 !" }); }
                int maxId = db.MeProducts.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                var images = db.Images.Where(m => m.ID == 0).FirstOrDefault();

                if (meProduct.Image != "empty")
                {
                    meProduct.ImageByte = images.ImageByte;
                }
                else
                {
                    //TempData["ErrMessage"] = "請確認有上傳圖片!";
                    return Json(new { ReturnStatus = "error", ReturnData = "請確認有上傳圖片 !" });
                }

                maxId += 1;
                meProduct.ID = maxId;
                meProduct.MeStockDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                meProduct.IsStock = false;
                meProduct.IsReturnMe = false;
                if (meProduct.Test == "empty")
                {
                    meProduct.ComList = "empty";
                }
                else
                {
                    List<Mutiplekpn> mutiplekpns = JsonSerializer.Deserialize<List<Mutiplekpn>>(meProduct.Test);
                    foreach (var item in mutiplekpns)
                    {
                        meProduct.ComList += item.value + ",";
                    }
                }

                //meProduct.ComList = JsonSerialize(meProduct.Test);

                db.MeProducts.Add(meProduct);

                if (meProduct.KindID == 0 || meProduct.KindProcessID == 0)
                {
                    //pass
                }
                else
                {
                    this.LabelRuleRepo.GenerateLabelRule(meProduct.KindID, meProduct.KindProcessID);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" + ex });
            }
        }

        public ActionResult ImageUpload(ImageViewModel model)
        {
            try
            {
                ManagementContextEntities db = new ManagementContextEntities();

                int imgId = 0;
                var images = db.Images.Where(m => m.ID == imgId).FirstOrDefault();

                var file = model.ImageFile;
                byte[] imagebyte = null;
                if (file != null)
                {
                    //file.SaveAs(Server.MapPath("/UploadImage/"+file.FileName));
                    BinaryReader reader = new BinaryReader(file.InputStream);
                    imagebyte = reader.ReadBytes(file.ContentLength);
                    images.Title = file.FileName;
                    images.ImageByte = imagebyte;
                    //images.ImagePath = "/UploadImage/" + file.FileName;
                    db.SaveChanges();

                }
                //ImageViewModel imageViewModel = new ImageViewModel();
                //byte[] imageBuff = imageViewModel.CreateThumbnailImage(100, 100, images.ImageByte, true);
                //return File(imageBuff, "image/png");
                return View();
                //return Json(imgId, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "ImageUpload(), ex:" + ex });
            }
        }

        public ActionResult DisplayingImage(int imgid)
        {
            try
            {
                ManagementContextEntities db = new ManagementContextEntities();
                var img = db.Images.SingleOrDefault(x => x.ID == imgid);
                byte[] imageBuff = { 136, 12 };
                if (!(img.ImageByte is null))
                {
                    ImageViewModel imageViewModel = new ImageViewModel();
                    imageBuff = imageViewModel.CreateThumbnailImage(100, 100, img.ImageByte, true);
                }                    

                return File(imageBuff, "image/png");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "DisplayingImage(), ex:" + ex });
            }
        }

        public ActionResult DisplayingIndexImage(int imgid)
        {
            try
            {
                ManagementContextEntities db = new ManagementContextEntities();
                var img = db.MeProducts.SingleOrDefault(x => x.ID == imgid);
                byte[] imageBuff = { 136, 12 };
                if (!(img.ImageByte is null))
                {
                    ImageViewModel imageViewModel = new ImageViewModel();
                    imageBuff = imageViewModel.CreateThumbnailImage(50, 50, img.ImageByte, true);
                }

                return File(imageBuff, "image/png");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "DisplayingIndexImage(), ex:" + ex });
            }
        }

        //public ActionResult upload()

        //{
        //    bool isSavedSuccessfully = true;
        //    string fname = "";
        //    try
        //    {
        //        foreach (string filename in Request.Files)
        //        {
        //            HttpPostedFileBase file = Request.Files[filename];
        //            fname = file.FileName;
        //            if (file!=null && file.ContentLength > 0)
        //            {
        //                var path = Path.Combine(Server.MapPath("~/uploadeimg"));
        //                string pathstring = Path.Combine(path.ToString());
        //                bool isexist = Directory.Exists(pathstring);
        //                if (!isexist)
        //                {
        //                    Directory.CreateDirectory(pathstring);
        //                }
        //                string uploadpath = string.Format("{0}\\{1}", pathstring, file.FileName);
        //                file.SaveAs(uploadpath);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        isSavedSuccessfully = false;
        //    }
        //    if (isSavedSuccessfully)
        //    {
        //        return Json(new
        //        {
        //            Message = fname
        //        });
        //    }
        //    else
        //    {
        //        return Json(new
        //        {
        //            Message = "Error in Saving file"
        //        });
        //    }
        //}

        [HttpGet]
        public JsonResult PostMethod2()
        {
            var kpnData = db.KPNs;

            List<Mutiplekpn> mutiplekpns = new List<Mutiplekpn>();

            foreach (var item in kpnData)
            {
                Mutiplekpn mutiplekpn = new Mutiplekpn();
                mutiplekpn.text = item.Name;
                mutiplekpn.value = item.Name;
                mutiplekpns.Add(mutiplekpn);
            }

            return Json(mutiplekpns, JsonRequestBehavior.AllowGet);
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

        private void loaddefault()
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

            Mutiplekpn mutiplekpn = new Mutiplekpn();
            List<Mutiplekpn> mutiplekpns = new List<Mutiplekpn>();

            foreach (var item in kpnData)
            {
                mutiplekpn.text = item.ID.ToString();
                mutiplekpn.value = item.Name;
                mutiplekpns.Add(mutiplekpn);
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

            string jsonmutiplekpns = JsonSerializer.Serialize(mutiplekpns);

            ViewBag.listKind = selectKindListItems;
            ViewBag.listSupplier = selectSupplierListItems;
            ViewBag.listCustomer = selectCustomerListItems;
            ViewBag.listKPN = jsonmutiplekpns;
            ViewBag.listOpid = selectOpidListItems;
            ViewBag.listProcessKind = selectProcessKindListItems;

        }

        private bool CheckInputErr(MeProduct meProduct)
        {
            if (meProduct.ProdName == null || meProduct.ProdName == string.Empty) { return true; };
            //if (meProduct.KindID == 0) { return true; };
            //if (meProduct.KindProcessID == 0) { return true; };
            if (meProduct.Opid == null) { return true; };
            //if (meProduct.SupplierID == 0) { return true; };
            //if (meProduct.CustomerID == 0) { return true; };
            if (meProduct.Image == "empty") { return true; };

            return false;
        }

    }
}