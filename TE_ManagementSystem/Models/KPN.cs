namespace TE_ManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KPN")]
    public partial class KPN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KPN()
        {
            MeProduct = new HashSet<MeProduct>();
        }

        [StringLength(20)]
        public string ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeProduct> MeProduct { get; set; }
    }
}
