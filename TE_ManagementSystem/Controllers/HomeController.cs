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
using System.Collections;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;

namespace TE_ManagementSystem.Controllers
{
    [Authorize]
    public class HomeController : ProjectBase
    {
        [Authorize(Users = "1,2,3,4")]
        public ActionResult Index()
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);

            //呼叫資料庫
            //DataTable dt = this.CallDatabase();

            // 回傳 Model
            //Hashtable outModel = new Hashtable(); //測試 Model
            //outModel.Add("Result", dt);

            //return View(outModel);

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

        /// <summary>
        /// 呼叫資料庫
        /// </summary>
        private DataTable CallDatabase()
        {
            // 記錄方法名稱
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);

            string connStr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ManagementContext1"].ConnectionString;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connStr;
            conn.Open();
            string sql = "select Opid from dbo.Employee where Opid = @UsrOpid "; //測試用 sql
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@UsrOpid", Session["UsrOpid"]);

            // 在執行 SQL 之前先記錄指令
            string sqlLog = sql;
            foreach (SqlParameter param in cmd.Parameters)
            {
                sqlLog = sqlLog.Replace(param.ParameterName.ToString(), "'" + param.Value.ToString() + "'");
            }
            this.logUtil.AppendMessage("SQL", sqlLog);

            SqlDataAdapter adpt = new SqlDataAdapter();
            adpt.SelectCommand = cmd;
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            DataTable dt = ds.Tables[0];
            // 省略...
            conn.Close();

            return dt;
        }

    }
}