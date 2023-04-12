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
            [Required(ErrorMessage = "Opid is required.")]
            [Display(Name = "工號")]
            public string Opid { get; set; }
            [Required(ErrorMessage = "Name is required.")]
            [Display(Name = "員工姓名")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Email is required.")]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Rank is required.")]
            [Display(Name = "權限")]
            [RegularExpression(@"^[1-5]$", ErrorMessage = "Your rank must be at only 1 number long and range 1~5")]
            public byte RankID { get; set; }
            [Required(ErrorMessage = "Department is required.")]
            [Display(Name = "部門")]
            public int DepartmentID { get; set; }
            [Required(ErrorMessage = "IsActive is required.")]
            [Display(Name = "在職狀態")]
            public bool IsActive { get; set; }
            [Required(ErrorMessage = "Password is required.")]
            [Display(Name = "Password")]
            [RegularExpression(@"(?=.*\d)(?=.*[A-Za-z]).{6,}", ErrorMessage = "Your password must be at least 6 characters long and contain at least 1 letter and 1 number")]
            public string Password { get; set; }
        }
    }
}