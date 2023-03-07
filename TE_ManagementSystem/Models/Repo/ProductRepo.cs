using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models.Repo
{
    public class ProductRepo : IProductRepo, IDisposable
    {
        public ProductContext db = new ProductContext();
        private bool disposedValue;

        public bool AddProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Product GetProductById(int id)
        {
            return db.Product.Find(id);
        }

        public IQueryable<Product> GetProductsByName(string name)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> ListAllProduct()
        {
            return db.Product;
        }

        public IQueryable<Product> ListAllProductUpdateDue()
        {
            DateTime dateTimeNow = DateTime.Now;
            DateTime dateTimeLastLend;
            TimeSpan timeSpan;

            foreach (var item in db.Product)
            {
                if (item.LastBorrowDate != null)
                {
                    dateTimeLastLend = (DateTime)item.LastBorrowDate;
                    timeSpan = dateTimeNow.Subtract(dateTimeLastLend);
                    int dayCount = timeSpan.Days;
                    if (dayCount>(int)item.MeProduct.ShiftTime)
                    {
                        item.Overdue = true;
                    }
                    else
                    {
                        item.Overdue = false;
                    }
                }
            }

            db.SaveChanges();
            return db.Product;
        }

        public Product LinkToResume(string id)
        {
            Product product = db.Product.Find(id);
            return product;
        }

        public bool UpdateProduct(Product product)
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