using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models.Repo
{
    public class SupplierRepo : ISupplierRepo, IDisposable
    {
        public ManagementContextEntities db = new ManagementContextEntities();
        private bool disposedValue;

        public bool AddSupplier(Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public bool DeleteSupplier(Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public Supplier GetSupplierById(int id)
        {
            return db.Suppliers.Find(id);
        }

        public Supplier GetSupplierByName(string name)
        {
            return db.Suppliers.Find(name);
        }

        public IQueryable<Supplier> ListAllSupplier()
        {
            return db.Suppliers;
        }

        public bool UpdateSupplier(Supplier supplier)
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