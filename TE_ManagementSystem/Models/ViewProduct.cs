using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models
{
    public class ViewProduct
    {
        public int ID { get; set; }
        public string NumberID { get; set; }
        public string RFID { get; set; }
        public string Status { get; set; }
        public int LocationID { get; set; }
        public int EngID { get; set; }
        public Nullable<System.DateTime> StockDate { get; set; }
        public int? Life { get; set; }
        public Nullable<System.DateTime> LastBorrowDate { get; set; }
        public Nullable<System.DateTime> LastReturnDate { get; set; }
        public Nullable<System.DateTime> UseLastDate { get; set; }
        public bool Usable { get; set; }
        public bool Overdue { get; set; }
        public string Spare1 { get; set; }
        public string Spare2 { get; set; }
        public string Spare3 { get; set; }
        public string Spare4 { get; set; }
        public string Spare5 { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateEmployee { get; set; }
        public string LocationName { get; set; }
        public string LocationRackPosition { get; set; }
        public string ProdName { get; set; }
        public string KindName { get; set; }
        public string KindProcessName { get; set; }
        public string SupplierName { get; set; }
        public string CustomerName { get; set; }
        public string ComList { get; set; }
        public int Quantity { get; set; }
        public bool Pb { get; set; }
        public string KindProcessNumber { get; set; }
        public string KindProcessNumberID { get; set; }
    }
}