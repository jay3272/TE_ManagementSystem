using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models.Repo
{
    public class PORepo : IPORepo, IDisposable
    {
        public ManagementContextEntities db = new ManagementContextEntities();
        private bool disposedValue;

        public bool AddProductTransaction(ProductTransaction productTransaction)
        {
            throw new NotImplementedException();
            //return 
        }

        public bool DeleteProductTransaction(ProductTransaction productTransaction)
        {
            throw new NotImplementedException();
        }
                
        public IQueryable<ProductTransaction> GetProductsTransactionByName(string name)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ProductTransaction> GetProductTransactionById(string id)
        {
            return db.ProductTransactions.DefaultIfEmpty().Where(p => p.Product.NumberID == id);
        }

        public List<ViewProductTransaction> ListAllProductTransaction()
        {
            DbSet<ProductTransaction> productTransactionSet = db.ProductTransactions; 
            List<ViewProductTransaction> viewProductTransactionList = new List<ViewProductTransaction>();

            foreach (var el in productTransactionSet)
            {
                viewProductTransactionList.Add(new ViewProductTransaction()
                {
                    ID=el.ID,
                    DepartmentName=el.Employee.Department.Name,
                    EmployeeName=el.Employee.Name,
                    ProductID=el.Product.NumberID,
                    ProdName=el.Product.MeProduct.ProdName,
                    KindName=el.Product.MeProduct.Kind.Name,
                    LocationName=el.Product.Location.Name,
                    LocationRackPosition=el.Product.Location.RackPosition,
                    ComList=el.Product.MeProduct.ComList,
                    IsReturn=el.IsReturn,
                    IsToFix=el.IsToFix,
                    BorrowDay=el.BorrowDay,
                    RegisterDate=el.RegisterDate,
                    KindProcessNumber = el.Product.MeProduct.KindProcess.Number,
                    KindProcessNumberID = el.Product.MeProduct.KindProcess.Number + el.Product.NumberID
                });
            }

            return viewProductTransactionList;
        }

        public bool UpdateProductTransaction(ProductTransaction productTransaction)
        {
            throw new NotImplementedException();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 處置受控狀態 (受控物件)
                    db.Dispose();
                }

                // TODO: 釋出非受控資源 (非受控物件) 並覆寫完成項
                // TODO: 將大型欄位設為 Null
                disposedValue=true;
            }
        }

        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}