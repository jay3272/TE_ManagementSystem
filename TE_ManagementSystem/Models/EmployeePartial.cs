using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models
{
    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {
        public class EmployeeMetadata
        {
            [Required(ErrorMessage = "required.")]
            [Display(Name = "工號")]
            public string Opid { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "員工姓名")]
            public string Name { get; set; }
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "權限")]
            [RegularExpression(@"^[1-4]$", ErrorMessage = "Your rank must be at only 1 number long and range 1~4")]
            public byte RankID { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "部門")]
            public int DepartmentID { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "在職狀態")]
            public bool IsActive { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "Password")]
            [DataType(DataType.Password)]
            [RegularExpression(@"^[0-9A-Za-z]{4}$", ErrorMessage = "Your password must be 4 characters long")]
            public string Password { get; set; }
            [Display(Name = "更新人員")]
            public string UpdateEmployee { get; set; }
        }
    }
}