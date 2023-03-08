namespace TE_ManagementSystem.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ProductContext : DbContext
    {
        public ProductContext()
            : base("name=ManagementContext")
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Kind> Kind { get; set; }
        public virtual DbSet<KindProcess> KindProcess { get; set; }
        public virtual DbSet<KPN> KPN { get; set; }
        public virtual DbSet<LabelRule> LabelRule { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<MeProduct> MeProduct { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductTransaction> ProductTransaction { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.CustCode)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Spare1)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Spare2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Spare3)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Spare4)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Spare5)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.MeProduct)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.Spare1)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.Spare2)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.Spare3)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.Spare4)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.Spare5)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.Employee)
                .WithRequired(e => e.Department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Opid)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.IsActive)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Spare1)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Spare2)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Spare3)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Spare4)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Spare5)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.MeProduct)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.ProductTransaction)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kind>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Kind>()
                .Property(e => e.Number)
                .IsUnicode(false);

            modelBuilder.Entity<Kind>()
                .Property(e => e.Spare1)
                .IsUnicode(false);

            modelBuilder.Entity<Kind>()
                .Property(e => e.Spare2)
                .IsUnicode(false);

            modelBuilder.Entity<Kind>()
                .Property(e => e.Spare3)
                .IsUnicode(false);

            modelBuilder.Entity<Kind>()
                .Property(e => e.Spare4)
                .IsUnicode(false);

            modelBuilder.Entity<Kind>()
                .Property(e => e.Spare5)
                .IsUnicode(false);

            modelBuilder.Entity<Kind>()
                .HasMany(e => e.LabelRule)
                .WithRequired(e => e.Kind)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kind>()
                .HasMany(e => e.MeProduct)
                .WithRequired(e => e.Kind)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KindProcess>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<KindProcess>()
                .Property(e => e.Number)
                .IsUnicode(false);

            modelBuilder.Entity<KindProcess>()
                .HasMany(e => e.LabelRule)
                .WithRequired(e => e.KindProcess)
                .HasForeignKey(e => e.ProcessKindID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KPN>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<KPN>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<KPN>()
                .Property(e => e.Spare1)
                .IsUnicode(false);

            modelBuilder.Entity<KPN>()
                .Property(e => e.Spare2)
                .IsUnicode(false);

            modelBuilder.Entity<KPN>()
                .Property(e => e.Spare3)
                .IsUnicode(false);

            modelBuilder.Entity<KPN>()
                .Property(e => e.Spare4)
                .IsUnicode(false);

            modelBuilder.Entity<KPN>()
                .Property(e => e.Spare5)
                .IsUnicode(false);

            modelBuilder.Entity<LabelRule>()
                .Property(e => e.LabelRule1)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.RackPosition)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.Spare1)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.Spare2)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.Spare3)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.Spare4)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.Spare5)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Product)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MeProduct>()
                .Property(e => e.ProdName)
                .IsUnicode(false);

            modelBuilder.Entity<MeProduct>()
                .Property(e => e.Opid)
                .IsUnicode(false);

            modelBuilder.Entity<MeProduct>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<MeProduct>()
                .Property(e => e.ComList)
                .IsUnicode(false);

            modelBuilder.Entity<MeProduct>()
                .Property(e => e.Spare1)
                .IsUnicode(false);

            modelBuilder.Entity<MeProduct>()
                .Property(e => e.Spare2)
                .IsUnicode(false);

            modelBuilder.Entity<MeProduct>()
                .Property(e => e.Spare3)
                .IsUnicode(false);

            modelBuilder.Entity<MeProduct>()
                .Property(e => e.Spare4)
                .IsUnicode(false);

            modelBuilder.Entity<MeProduct>()
                .Property(e => e.Spare5)
                .IsUnicode(false);

            modelBuilder.Entity<MeProduct>()
                .HasMany(e => e.Product)
                .WithRequired(e => e.MeProduct)
                .HasForeignKey(e => e.EngID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.NumberID)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.RFID)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Life)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Spare1)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Spare2)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Spare3)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Spare4)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Spare5)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductTransaction)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.ProductID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductTransaction>()
                .Property(e => e.Opid)
                .IsUnicode(false);

            modelBuilder.Entity<ProductTransaction>()
                .Property(e => e.ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<ProductTransaction>()
                .Property(e => e.Spare1)
                .IsUnicode(false);

            modelBuilder.Entity<ProductTransaction>()
                .Property(e => e.Spare2)
                .IsUnicode(false);

            modelBuilder.Entity<ProductTransaction>()
                .Property(e => e.Spare3)
                .IsUnicode(false);

            modelBuilder.Entity<ProductTransaction>()
                .Property(e => e.Spare4)
                .IsUnicode(false);

            modelBuilder.Entity<ProductTransaction>()
                .Property(e => e.Spare5)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Spare2)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Spare3)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Spare4)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Spare5)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.MeProduct)
                .WithRequired(e => e.Supplier)
                .WillCascadeOnDelete(false);
        }
    }
}
