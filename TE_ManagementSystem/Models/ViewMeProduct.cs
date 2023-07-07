using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models
{
    public class ViewMeProduct
    {
        public int ID { get; set; }
        public string ProdName { get; set; }
        public int KindID { get; set; }
        public int KindProcessID { get; set; }
        public int CustomerID { get; set; }
        public int SupplierID { get; set; }
        public string Opid { get; set; }
        public int Quantity { get; set; }
        public int ShiftTime { get; set; }
        public bool Pb { get; set; }
        public string Image { get; set; }
        public string ComList { get; set; }
        public string Spare1 { get; set; }
        public Nullable<System.DateTime> MeStockDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateEmployee { get; set; }
        public string KindProcessName { get; set; }
        public string KindName { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string OpidName { get; set; }

    }
}