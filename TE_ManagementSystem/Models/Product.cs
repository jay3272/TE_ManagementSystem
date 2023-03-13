//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TE_ManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.ProductTransactions = new HashSet<ProductTransaction>();
        }
    
        public int ID { get; set; }
        public string NumberID { get; set; }
        public string RFID { get; set; }
        public string Status { get; set; }
        public int LocationID { get; set; }
        public int EngID { get; set; }
        public Nullable<System.DateTime> StockDate { get; set; }
        public string Life { get; set; }
        public Nullable<System.DateTime> LastBorrowDate { get; set; }
        public Nullable<System.DateTime> LastReturnDate { get; set; }
        public Nullable<System.DateTime> UseLastDate { get; set; }
        public bool Usable { get; set; }
        public bool Overdue { get; set; }
        public string Spare1 { get; set; }
        public string Spare2 { get; set; }
        public string Spare3 { get; set; }
        public string Spare4 { get; set; }
        public string Spare5 { get; set; }
    
        public virtual Location Location { get; set; }
        public virtual MeProduct MeProduct { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductTransaction> ProductTransactions { get; set; }
    }
}
