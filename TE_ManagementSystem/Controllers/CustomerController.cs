using Castle.Core.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_ManagementSystem.Models;
using TE_ManagementSystem.Models.Repo;

namespace TE_ManagementSystem.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        ICustomerRepo CustomerRepo = new CustomerRepo();
        private ManagementContextEntities db = new ManagementContextEntities();

        // GET: Customer
        [Authorize(Users="1,2,3,4,5")]
        public ActionResult Index()
        {
            return View(CustomerRepo.ListAllCustomer());
        }

        [Authorize(Users = "1,2")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Users = "1,2")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,CustCode,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateEmployee")] Customer customer)
        {
            try
            {
                if (this.CheckInputErr(customer)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }

                int maxId = db.Customers.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                maxId += 1;
                customer.ID = maxId;
                customer.UpdateEmployee = Session["UsrName"].ToString();

                db.Customers.Add(customer);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" + ex });
            }
        }

        [Authorize(Users = "1,2,3")]
        public ActionResult Edit(int id)
        {
            try
            {
                var model = db.Customers.Where(x => x.ID == id).FirstOrDefault();

                return View(model);
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "Edit(), ex:" + ex });
            }
        }

        [HttpPost]
        [Authorize(Users = "1,2,3")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,CustCode,Spare1,Spare2,Spare3,Spare4,Spare5,UpdateDate,UpdateEmployee")] Customer customer)
        {
            try
            {
                if (this.CheckInputErr(customer)) { return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整 !" }); }
                var model = db.Customers.Where(x => x.ID == customer.ID).FirstOrDefault();
                model.Name = customer.Name;
                model.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                model.UpdateEmployee = Session["UsrName"].ToString();

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { ReturnStatus = "error", ReturnData = "請確認輸入訊息完整或資料重複 !" + ex });
            }
        }

        private bool CheckInputErr(Customer customer)
        {
            if (customer.Name == null || customer.Name == string.Empty) { return true; };
            if (customer.CustCode == null || customer.CustCode == string.Empty) { return true; };

            return false;
        }

    }
}