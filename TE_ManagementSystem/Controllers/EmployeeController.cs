using Castle.Core.Resource;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;

namespace TE_ManagementSystem.Controllers
{
    [Authorize]
    public class EmployeeController : ProjectBase
    {
        IEmployeeRepo EmployeeRepo = new EmployeeRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Employee
        [Authorize(Users = "1,2,3,4,5")]
        public ActionResult Index()
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);
            return View(EmployeeRepo.ListAllEmployee());
        }

        [Authorize(Users = "1,2")]
        public ActionResult Create()
        {
            this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);

            GlobalValue.LoginUserName = Convert.ToString(Session["UsrName"] ?? "").Trim();

            if (GlobalValue.LoginUserName.ToString().Count() > 0)
            {
                this.loaddefault();
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
        public ActionResult Create([Bind(Include = "Opid,Name,Email,RankID,DepartmentID,IsActive,Password,UpdateEmployee")] Employee employee)
        {
            try
            {
                if (this.CheckInputErr(employee)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }

                //employee.Password = Encryption.Encrypt(employee.Password, "d3A#");
                employee.Opid = employee.Opid.Trim();
                employee.Name = employee.Name.Trim();
                employee.Password = employee.Password.Trim();

                try
                {
                    if (Session["UsrName"].ToString().Count() > 0)
                    {
                        employee.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        employee.UpdateEmployee = Convert.ToString(Session["UsrName"] ?? "").Trim();
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

                employee.IsActive = true;
                if (employee.Email == null)
                {
                    employee.Email = "NA";
                }
                else
                {
                    employee.Email = employee.Email.Trim() + "@tailyn.com.tw";
                }

                db.Employees.Add(employee);

                db.SaveChanges();
                this.logUtil.AppendMethod("Save Create");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !"});
            }
        }

        [Authorize(Users = "1,2")]
        public ActionResult Edit(string Opid)
        {
            try
            {
                this.logUtil.AppendMethod(MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + MethodBase.GetCurrentMethod().Name);

                GlobalValue.LoginUserName = Convert.ToString(Session["UsrName"] ?? "").Trim();

                if (GlobalValue.LoginUserName.ToString().Count() > 0)
                {
                    this.loaddefault();
                    var model = db.Employees.Where(x => x.Opid == Opid).FirstOrDefault();
                    return View(model);
                }
                else
                {
                    return Json(new { ReturnStatus = "error", ReturnData = "登入逾時...請重新登入再匯入 !" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "Edit(), ex:" + ex });
            }
        }

        [HttpPost]
        [Authorize(Users = "1,2")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Opid,Name,Email,RankID,DepartmentID,IsActive,Password,UpdateDate,UpdateEmployee")] Employee employee)
        {
            try
            {
                if (this.CheckInputEditErr(employee)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }
                var model = db.Employees.Where(x => x.Opid == employee.Opid).FirstOrDefault();
                model.Name = employee.Name.Trim();
                model.Email = employee.Email.Trim();
                if (employee.RankID != 0) { model.RankID = employee.RankID; }
                if (employee.DepartmentID !=0) { model.DepartmentID = employee.DepartmentID; }
                model.IsActive = true;
                model.Password = employee.Password.Trim();
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

        private bool CheckInputErr(Employee employee)
        {
            if (employee.Opid == null || employee.Opid == string.Empty) { return true; };
            if (employee.Name == null || employee.Name == string.Empty) { return true; };
            if (employee.RankID == 0) { return true; };
            if (employee.DepartmentID == 0) { return true; };

            return false;
        }

        private bool CheckInputEditErr(Employee employee)
        {
            if (employee.Opid == null || employee.Opid == string.Empty) { return true; };
            if (employee.Name == null || employee.Name == string.Empty) { return true; };

            return false;
        }

        private void loaddefault()
        {
            ViewBag.Department = db.Departments;

            var departmentData = db.Departments;

            List<SelectListItem> selectDepartmentListItems = new List<SelectListItem>();
            List<SelectListItem> selectRankListItems = new List<SelectListItem>();

            foreach (var item in departmentData)
            {
                selectDepartmentListItems.Add(new SelectListItem()
                {
                    Text = item.ID + "/" + item.Name,
                    Value = item.ID.ToString(),
                    Selected = false
                });
            }

            var dictRank = new Dictionary<string, string>();
            dictRank.Add("1", "Admin");
            dictRank.Add("2", "Supervisor");
            dictRank.Add("3", "Engineer");
            dictRank.Add("4", "Operator");
            dictRank.Add("5", "Guest");

            foreach (KeyValuePair<string, string> item in dictRank)
            {
                selectRankListItems.Add(new SelectListItem()
                {
                    Text = item.Key + "/" + item.Value,
                    Value = item.Key.ToString(),
                    Selected = false
                });
            }
            
            ViewBag.listDepartment = selectDepartmentListItems;
            ViewBag.listRank = selectRankListItems;

        }

    }
}