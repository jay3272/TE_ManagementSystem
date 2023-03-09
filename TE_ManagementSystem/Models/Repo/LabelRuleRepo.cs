using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models.Repo
{
    public class LabelRuleRepo : ILabelRuleRepo, IDisposable
    {
        public ProductContext db = new ProductContext();
        private bool disposedValue;

        public bool AddLabelRule(LabelRule labelRule)
        {
            throw new NotImplementedException();
        }

        public bool DeleteLabelRule(LabelRule labelRule)
        {
            throw new NotImplementedException();
        }

        public LabelRule GetLabelRuleById(int id)
        {
            return db.LabelRule.Find(id);
        }

        public LabelRule GetLabelRuleByName(string name)
        {
            return db.LabelRule.Find(name);
        }

        public IQueryable<LabelRule> ListAllLabelRule()
        {
            return db.LabelRule;
        }

        public bool UpdateLabelRule(LabelRule labelRule)
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