namespace TE_ManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductTransaction")]
    public partial class ProductTransaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Opid { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductID { get; set; }

        public bool IsReturn { get; set; }

        public bool IsToFix { get; set; }

        public int BorrowDay { get; set; }

        public DateTime RegisterDate { get; set; }

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

        public virtual Employee Employee { get; set; }

        public virtual Product Product { get; set; }
    }
}
