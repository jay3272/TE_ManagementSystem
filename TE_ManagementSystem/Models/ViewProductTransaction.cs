using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models
{
    public class ViewProductTransaction
    {
        public int ID { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeName { get; set; }

        public string Opid { get; set; }
        public string ProductID { get; set; }
        public string ProdName { get; set; }
        public string KindName { get; set; }
        public string LocationName { get; set; }
        public string LocationRackPosition { get; set; }
        public string ComList { get; set; }
        public bool IsReturn { get; set; }
        public bool IsToFix { get; set; }
        public int BorrowDay { get; set; }
        public System.DateTime RegisterDate { get; set; }
        public string Spare1 { get; set; }
        public string Spare2 { get; set; }
        public string Spare3 { get; set; }
        public string Spare4 { get; set; }
        public string Spare5 { get; set; }

    }
}