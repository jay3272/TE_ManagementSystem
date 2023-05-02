using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace TE_ManagementSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [Authorize(Users = "1,2,3,4")]
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

        [HttpPost]
        public async Task <ActionResult> Chat(FormCollection collection)
        {
            string text = collection["text"];

            var query = text;
            var apiUrl = $"https://chatgp.azurewebsites.net/api/Chat/UseChatGPT?query={query}";

            HttpClient httpClient = new HttpClient();
            var client = httpClient;
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Json(new {text = responseContent});
            }
            else
            {
                return Json(new { text = "Failed to call API. Status code: " + response.StatusCode });
            }   

        }
        
    }
}