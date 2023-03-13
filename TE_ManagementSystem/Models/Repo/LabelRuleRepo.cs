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

        public string GetLabelRule(int engId)
        {
            var meProduct = db.MeProduct.Where
            (x => (x.ID == engId)).FirstOrDefault();

            var labelRule = db.LabelRule.Where
            (k => (k.KindID == meProduct.KindID && k.ProcessKindID == meProduct.KindProcessID)).FirstOrDefault();

            return labelRule.LabelRule1;
        }

        public IQueryable<LabelRule> ListAllLabelRule()
        {
            return db.LabelRule;
        }

        public bool UpdateLabelRuleNumber(int id, string name)
        {
            try
            {
                LabelRule labelRule = new LabelRule();
                int kindId = this.FindKindId(id);
                int processkindId = this.FindProcessKindId(id);

                var record = db.LabelRule.Where
                (k => (k.KindID == kindId && k.ProcessKindID == processkindId)).FirstOrDefault();

                //確認檢查
                string recordFrontNum = record.LabelRule1.Substring(0, 6);
                string newlabelFrontNum = name.Substring(0, 6);

                if (recordFrontNum == newlabelFrontNum)
                {
                    db.LabelRule.Find(record.ID).LabelRule1 = name;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    //check bug
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
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

        private int FindKindId(int id)
        {
            var recordMe = db.MeProduct.DefaultIfEmpty().Where(m => m.ID == id);

            foreach (var el in recordMe)
            {
                return el.KindID;
            }
            return -1;
        }

        private int FindProcessKindId(int id)
        {
            var recordMe = db.MeProduct.DefaultIfEmpty().Where(m => m.ID == id);

            foreach (var el in recordMe)
            {
                return el.KindProcessID;
            }
            return -1;
        }

    }
}