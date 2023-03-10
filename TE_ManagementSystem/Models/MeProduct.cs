namespace TE_ManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MeProduct")]
    public partial class MeProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MeProduct()
        {
            Product = new HashSet<Product>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProdName { get; set; }

        public int KindID { get; set; }

        public int KindProcessID { get; set; }

        public int CustomerID { get; set; }

        public int SupplierID { get; set; }

        [Required]
        [StringLength(20)]
        public string Opid { get; set; }

        public int Quantity { get; set; }

        public int ShiftTime { get; set; }

        public bool IsStock { get; set; }

        public bool IsReturnMe { get; set; }

        public bool Pb { get; set; }

        [Required]
        [StringLength(100)]
        public string Image { get; set; }

        [Required]
        [StringLength(100)]
        public string ComList { get; set; }

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

        public virtual Customer Customer { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Kind Kind { get; set; }

        public virtual KindProcess KindProcess { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Product { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
