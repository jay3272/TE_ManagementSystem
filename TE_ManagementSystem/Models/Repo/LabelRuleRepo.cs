using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models.Repo
{
    public class LabelRuleRepo : ILabelRuleRepo, IDisposable
    {
        public ManagementContextEntities db = new ManagementContextEntities();
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
            return db.LabelRules.Find(id);
        }

        public LabelRule GetLabelRuleByName(string name)
        {
            return db.LabelRules.Find(name);
        }

        public string GetLabelRule(int engId)
        {
            var meProduct = db.MeProducts.Where
            (x => (x.ID == engId)).FirstOrDefault();

            var labelRule = db.LabelRules.Where
            (k => (k.KindID == meProduct.KindID )).FirstOrDefault();

            return labelRule.LabelRule1;
        }

        //public string GetDefaultLabelRule()
        //{            
        //    var labelRule = db.LabelRules.Where
        //    (k => (k.ID == 0)).FirstOrDefault();

        //    return labelRule.LabelRule1;
        //}

        public int GetLocationID(string room, string rack)
        {
            var location = db.Locations.Where
            (x => (x.Name == room && x.RackPosition == rack)).FirstOrDefault();

            return location.ID;
        }

        public bool GenerateLabelRule(int kindId)
        {
            LabelRule labelRule = new LabelRule();
            int maxlabelRuleId = db.LabelRules.DefaultIfEmpty().Max(r => r == null ? 0 : r.ID);
            maxlabelRuleId += 1;
            labelRule.ID = maxlabelRuleId;
            labelRule.KindID = kindId;

            var chkRepeat = db.LabelRules.Where
            (k => (k.KindID == labelRule.KindID )).FirstOrDefault();

            if (chkRepeat is null)
            {
                var kinds = db.Kinds.Where
                (k => k.ID == labelRule.KindID).FirstOrDefault();

                labelRule.LabelRule1 = kinds.Number + "00000";
                db.LabelRules.Add(labelRule);
                db.SaveChanges();
                return true;
            }
            else
            {
                //已有此規則不需建立新的
                return false;
            }

        }

        ///// <summary>
        ///// 給手動匯入資料庫檢查使用
        ///// </summary>
        ///// <param name="engID"></param>
        ///// <returns></returns>
        //public bool GenerateLabelRule(int engID)
        //{
        //    MeProduct meProduct = new MeProduct();
        //    var meProd = db.MeProducts.Where
        //        (m => m.ID == engID).FirstOrDefault();

        //    LabelRule labelRule = new LabelRule();
        //    int maxlabelRuleId = db.LabelRules.DefaultIfEmpty().Max(r => r == null ? 0 : r.ID);
        //    maxlabelRuleId += 1;
        //    labelRule.ID = maxlabelRuleId;
        //    labelRule.KindID = meProd.KindID;

        //    var chkRepeat = db.LabelRules.Where
        //    (k => (k.KindID == labelRule.KindID )).FirstOrDefault();

        //    if (chkRepeat is null)
        //    {
        //        var kinds = db.Kinds.Where
        //        (k => k.ID == labelRule.KindID).FirstOrDefault();

        //        labelRule.LabelRule1 = kinds.Number + "00000";
        //        db.LabelRules.Add(labelRule);
        //        db.SaveChanges();
        //        return true;
        //    }
        //    else
        //    {
        //        //已有此規則不需建立新的
        //        return false;
        //    }
        //}

        public IQueryable<LabelRule> ListAllLabelRule()
        {
            return db.LabelRules;
        }

        public bool UpdateLabelRuleNumber(int id, string name)
        {
            try
            {
                LabelRule labelRule = new LabelRule();
                int kindId = this.FindKindId(id);
                int processkindId = this.FindProcessKindId(id);

                var record = db.LabelRules.Where
                (k => (k.KindID == kindId )).FirstOrDefault();

                //確認檢查
                string recordFrontNum = record.LabelRule1.Substring(0, 6);
                string newlabelFrontNum = name.Substring(0, 6);

                if (recordFrontNum.Equals(newlabelFrontNum))
                {
                    db.LabelRules.Find(record.ID).LabelRule1 = name;
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
                disposedValue = true;
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
            var recordMe = db.MeProducts.DefaultIfEmpty().Where(m => m.ID == id);

            foreach (var el in recordMe)
            {
                return el.KindID;
            }
            return -1;
        }

        private int FindProcessKindId(int id)
        {
            var recordMe = db.MeProducts.DefaultIfEmpty().Where(m => m.ID == id);

            foreach (var el in recordMe)
            {
                return el.KindProcessID;
            }
            return -1;
        }

    }
}