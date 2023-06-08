using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models
{
    public class GlobalValue
    {
        public static string LoginUserName { get; set; }
        public static string LoginUserRank { get; set; }
        public static List<ViewProduct> viewproductsList { get; set; } 
    }
}