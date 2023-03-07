﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models.Repo
{
    public class MeProductRepo : IMeProductRepo, IDisposable
    {
        public ProductContext db = new ProductContext();
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
            var record = db.MeProduct.DefaultIfEmpty().Where(p => p.IsStock == false);

            //foreach (var item in record)
            //{
            //    string tmp = item.ProdName + "_" + item.IsStock;
            //}

            return record;
        }

        public bool UpdateMeProductStock(int id)
        {
            try
            {
                //var record = db.MeProduct.Find(id);
                db.MeProduct.Find(id).IsStock = true;
                //record.IsStock = true;
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
            return db.MeProduct.Find(id);
        }

        public MeProduct GetMeProductsByName(string name)
        {
            return db.MeProduct.Find(name);
        }

        public IQueryable<MeProduct> ListAllMeProduct()
        {
            return db.MeProduct;
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