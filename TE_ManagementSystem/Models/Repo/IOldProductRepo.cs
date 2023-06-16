using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE_ManagementSystem.Models.Repo
{
    interface IOldProductRepo
    {
        //列出所以ME Product清單
        List<ViewOldProduct> ListAllOldProduct();

        //列出所以ME Product清單
        IQueryable<OldProduct> ListAllOldProductNotDone();

        //更新
        bool UpdateOldProductDone(int id);

    }
}
