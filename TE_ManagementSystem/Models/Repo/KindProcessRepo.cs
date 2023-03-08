using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models.Repo
{
    public class KindProcessRepo : IKindProcessRepo, IDisposable
    {
        public ProductContext db = new ProductContext();
        private bool disposedValue;

        public bool AddKindProcess(KindProcess kindProcess)
        {
            throw new NotImplementedException();
        }

        public bool DeleteKindProcess(KindProcess kindProcess)
        {
            throw new NotImplementedException();
        }

        public KindProcess GetKindProcessById(int id)
        {
            return db.KindProcess.Find(id);
        }

        public KindProcess GetKindProcessByName(string name)
        {
            return db.KindProcess.Find(name);
        }

        public IQueryable<KindProcess> ListAllKindProcess()
        {
            return db.KindProcess;
        }

        public bool UpdateKindProcess(KindProcess kindProcess)
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