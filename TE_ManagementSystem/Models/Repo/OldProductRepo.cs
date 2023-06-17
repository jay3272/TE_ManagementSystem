using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models.Repo
{
    public class OldProductRepo : IOldProductRepo, IDisposable
    {
        public ManagementContextEntities db = new ManagementContextEntities();
        private bool disposedValue;

        public List<ViewOldProduct> ListAllOldProduct()
        {
            var oldproductSet = db.OldProducts.DefaultIfEmpty().Where(p => p.IsDone != true);
            List<ViewOldProduct> oldproductsList = new List<ViewOldProduct>();

            foreach (var el in oldproductSet)
            {
                oldproductsList.Add(new ViewOldProduct()
                {
                    ID=el.ID,
                    Number=el.Number,
                    NumberID=el.NumberID,
                    Location=el.Location,
                    ProdName=el.ProdName,
                    Files=el.Files,
                    Customer=el.Customer,
                    Status=el.Status,
                    Qty=el.Qty,
                    StatusOnToolRoom=el.StatusOnToolRoom,
                    KPN=el.KPN,
                    KindProcess=el.KindProcess,
                    Kind=el.Kind
                });
            }

            return oldproductsList;
        }

        public IQueryable<OldProduct> ListAllOldProductNotDone()
        {
            var record = db.OldProducts.DefaultIfEmpty().Where(p => p.IsDone != true);

            return record;
        }

        public bool UpdateOldProductDone(int id)
        {
            try
            {
                var oldProduct = db.OldProducts.Where
                (m => m.ID == id).FirstOrDefault();

                oldProduct.IsDone = true;

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
                disposedValue = true;
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