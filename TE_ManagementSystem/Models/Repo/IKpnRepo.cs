using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE_ManagementSystem.Models.Repo
{
    interface IKpnRepo
    {
        //列出清單
        IQueryable<KPN> ListAllKpn();

        //列出某個紀錄
        KPN GetKpnById(int id);

        //搜尋
        KPN GetKpnByName(string name);

        //新增
        bool AddKpn(KPN kpn);
        //更新
        bool UpdateKpn(KPN kpn);

        //刪除
        bool DeleteKpn(KPN kpn);

    }
}
