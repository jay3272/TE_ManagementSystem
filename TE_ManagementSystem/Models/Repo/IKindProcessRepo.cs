using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE_ManagementSystem.Models.Repo
{
    interface IKindProcessRepo
    {
        //列出清單
        IQueryable<KindProcess> ListAllKindProcess();

        //列出某個紀錄
        KindProcess GetKindProcessById(int id);

        //搜尋
        KindProcess GetKindProcessByName(string name);

        //新增
        bool AddKindProcess(KindProcess kindProcess);
        //更新
        bool UpdateKindProcess(KindProcess kindProcess);

        //刪除
        bool DeleteKindProcess(KindProcess kindProcess);

    }
}
