namespace TE_ManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            ProductTransaction = new HashSet<ProductTransaction>();
        }

        public int ID { get; set; }

        [Key]
        [StringLength(50)]
        public string NumberID { get; set; }

        [StringLength(50)]
        public string RFID { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        public int LocationID { get; set; }

        public int EngID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StockDate { get; set; }

        [StringLength(10)]
        public string Life { get; set; }

        public DateTime? LastBorrowDate { get; set; }

        public DateTime? LastReturnDate { get; set; }

        public DateTime? UseLastDate { get; set; }

        public bool Usable { get; set; }

        public bool Overdue { get; set; }

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

        public virtual Location Location { get; set; }

        public virtual MeProduct MeProduct { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductTransaction> ProductTransaction { get; set; }
    }
}
