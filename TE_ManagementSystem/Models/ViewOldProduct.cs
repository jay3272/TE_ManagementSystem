using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models
{
    public class ViewOldProduct
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public string NumberID { get; set; }
        public string Location { get; set; }
        public string ProdName { get; set; }
        public string Files { get; set; }
        public string Customer { get; set; }
        public string Status { get; set; }
        public string Qty { get; set; }
        public string StatusOnToolRoom { get; set; }
        public string KPN { get; set; }
        public string KindProcess { get; set; }
        public Nullable<bool> IsDone { get; set; }
    }
}