using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE_ManagementSystem.Models.Repo
{
    interface IKindRepo
    {
        //列出所以ME Product清單
        IQueryable<Kind> ListAllKind();

        //列出某個ME Product紀錄
        Kind GetKindById(int id);

        //搜尋
        Kind GetKindByName(string name);

        ////新增
        //string GetLabelRuleByName(int id);

        ////更新
        //bool UpdateLabelRule(int id,string name);

        //新增
        bool AddKind(Kind kind);
        //更新
        bool UpdateKind(Kind kind);

        //刪除
        bool DeleteKind(Kind kind);

    }
}
