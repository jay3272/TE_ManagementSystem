using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models.Repo
{
    public class MeProductRepo : IMeProductRepo, IDisposable
    {
        public ManagementContextEntities db = new ManagementContextEntities();
        private bool disposedValue;

        public bool AddMeProduct(MeProduct meProduct)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMeProduct(MeProduct meProduct)
        {
            throw new NotImplementedException();
        }

        public IQueryable<MeProduct> ListAllMeProductNotStock()
        {
            var record = db.MeProducts.DefaultIfEmpty().Where(p => p.IsStock == false);

            //foreach (var item in record)
            //{
            //    string tmp = item.ProdName + "_" + item.IsStock;
            //}

            return record;
        }

        public bool CheckNameRepeat(string name)
        {
            try
            {
                int count = db.MeProducts.Where(p => p.ProdName == name).Count();

                if (count >0)
                {
                    return true;
                }
                else
                {
                    return false;
                }                
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateMeProductStock(int id)
        {
            try
            {

                var meProduct = db.MeProducts.Where
                (m => m.ID == id).FirstOrDefault();

                meProduct.IsStock = true;
                
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public MeProduct GetMeProductById(int id)
        {
            return db.MeProducts.Find(id);
        }

        public IQueryable<MeProduct> GetMeProductsByName(string name)
        {
            return db.MeProducts.Where(x => x.ProdName == name);
        }

        public int GetMeProductIdByName(string name)
        {
            return db.MeProducts.Where(x => x.ProdName == name).FirstOrDefault().ID;
        }

        public IQueryable<MeProduct> ListAllMeProduct()
        {
            return db.MeProducts;
        }

        public bool UpdateMeProduct(MeProduct meProduct)
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