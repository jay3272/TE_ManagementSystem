using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;

namespace TE_ManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public JsonResult ImageUpload(ImageViewModel model)
        {
            ManagementContextEntities db = new ManagementContextEntities();
            int imgId = 0;
            var file = model.ImageFile;
            byte[] imagebyte = null;
            if (file != null)
            {
                file.SaveAs(Server.MapPath("/UploadImage/"+file.FileName));
                BinaryReader reader = new BinaryReader(file.InputStream);
                imagebyte = reader.ReadBytes(file.ContentLength);
                Image img = new Image();
                img.Title = file.FileName;
                img.ImageByte = imagebyte;
                img.ImagePath = "/UploadImage/" + file.FileName;
                db.Images.Add(img);
                db.SaveChanges();
                imgId = img.ID;
            }
            return Json(imgId, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DisplayingImage(int imgid)
        {
            ManagementContextEntities db = new ManagementContextEntities();

            var img = db.Images.SingleOrDefault(x => x.ID == imgid);
            return File(img.ImageByte, "image/jpg");
        }
    }
}