namespace TE_ManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Employee")]
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            MeProduct = new HashSet<MeProduct>();
            ProductTransaction = new HashSet<ProductTransaction>();
        }

        [Key]
        [StringLength(20)]
        public string Opid { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        public byte RankID { get; set; }

        public int DepartmentID { get; set; }

        [Required]
        [StringLength(2)]
        public string IsActive { get; set; }

        [StringLength(100)]
        public string Spare1 { get; set; }

        [StringLength(100)]
        public string Spare2 { get; set; }

        [StringLength(100)]
        public string Spare3 { get; set; }

        [StringLength(100)]
        public string Spare4 { get; set; }

        [StringLength(100)]
        public string Spare5 { get; set; }

        public virtual Department Department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeProduct> MeProduct { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductTransaction> ProductTransaction { get; set; }
    }
}
