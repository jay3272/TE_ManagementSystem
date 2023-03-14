using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE_ManagementSystem.Models.Repo
{
    interface ILocationRepo
    {
        //列出清單
        IQueryable<Location> ListAllLocation();

        //列出某個紀錄
        Location GetLocationById(int id);

        //搜尋
        Location GetLocationByName(string name);

        //新增
        bool AddLocation(Location location);
        //更新
        bool UpdateLocation(Location location);

        //刪除
        bool DeleteLocation(Location location);

    }
}
