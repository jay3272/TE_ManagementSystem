using System;
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
    public class MeProductController : Controller
    {
        IMeProductRepo MeProductRepo = new MeProductRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: MeProduct
        [Authorize(Users = "1,2,3,4")]
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
        public ActionResult Create([Bind(Include = "ID,ProdName,KindID,KindProcessID,CustomerID,SupplierID,Opid,Quantity,ShiftTime,Pb,Image,ComList,Spare1,Spare2,Spare3,Spare4,Spare5,Test,ImageByte")] MeProduct meProduct)
        {
            int maxId = db.MeProducts.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
            var images = db.Images.Where(m => m.ID == 0).FirstOrDefault();

            if (images.Title == meProduct.Image)
            {
                meProduct.ImageByte = images.ImageByte;
            }
            else
            {
                TempData["ErrMessage"] = "請確認有上傳圖片!";
                this.loaddefault();
                return View();
            }

            maxId += 1;
            meProduct.ID = maxId;
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

        public JsonResult ImageUpload(ImageViewModel model)
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
            return Json(imgId, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DisplayingImage(int imgid)
        {
            ManagementContextEntities db = new ManagementContextEntities();
            var img = db.Images.SingleOrDefault(x => x.ID == imgid);

            ImageViewModel imageViewModel = new ImageViewModel();
            byte[] imageBuff = imageViewModel.CreateThumbnailImage(100, 100, img.ImageByte, true);

            return File(imageBuff, "image/png");
        }

        public ActionResult DisplayingIndexImage(int imgid)
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


    }
}