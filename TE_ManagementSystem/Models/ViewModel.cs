using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TE_ManagementSystem.Models
{
    public class ViewModel
    {
        public string Name { get; set; }
        public IEnumerable<SelectListItem> MyList { get; set; }
    }
}