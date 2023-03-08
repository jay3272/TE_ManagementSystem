namespace TE_ManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LabelRule")]
    public partial class LabelRule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int ProcessKindID { get; set; }

        public int KindID { get; set; }

        [Column("LabelRule")]
        [Required]
        [StringLength(100)]
        public string LabelRule1 { get; set; }

        public virtual Kind Kind { get; set; }

        public virtual KindProcess KindProcess { get; set; }
    }
}
