using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models.Repo
{
    public class KindRepo : IKindRepo, IDisposable
    {
        public ProductContext db = new ProductContext();
        private bool disposedValue;

        public bool AddKind(Kind kind)
        {
            throw new NotImplementedException();
        }

        public bool DeleteKind(Kind kind)
        {
            throw new NotImplementedException();
        }

        public Kind GetKindById(int id)
        {
            return db.Kind.Find(id);
        }

        public Kind GetKindByName(string name)
        {
            return db.Kind.Find(name);
        }

        //public string GetLabelRuleByName(int id)
        //{
        //    string kindName =  this.FindKindName(id);

        //    if (kindName != string.Empty)
        //    {
        //        var record = db.Kind.DefaultIfEmpty().Where(x => x.Name == kindName);
        //        string labelrule;

        //        int count = record.Count();

        //        if (count == 1)
        //        {
        //            foreach (var item in record)
        //            {
        //                labelrule = item.LabelRule;
        //                return labelrule;
        //            }
        //        }
        //    }

        //    return string.Empty;
        //}

        public IQueryable<Kind> ListAllKind()
        {
            return db.Kind;
        }

        public bool UpdateKind(Kind kind)
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


        private string FindKindName(int id)
        {
            var recordMe = db.MeProduct.DefaultIfEmpty().Where(m => m.ID == id);
            
            foreach (var el in recordMe)
            {
                return el.Kind.Name;
            }
            return string.Empty;
        }

        private int FindKindId(int id)
        {
            var recordMe = db.MeProduct.DefaultIfEmpty().Where(m => m.ID == id);

            foreach (var el in recordMe)
            {
                return el.Kind.ID;
            }
            return -1;
        }


    }
}