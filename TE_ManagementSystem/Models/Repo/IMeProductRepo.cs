using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE_ManagementSystem.Models.Repo
{
    interface IMeProductRepo
    {
        //列出所以ME Product清單
        List<ViewMeProduct> ListAllMeProduct();

        //列出所以ME Product清單
        IQueryable<MeProduct> ListAllMeProductNotStock();

        //列出某個ME Product紀錄
        MeProduct GetMeProductById(int id);

        //搜尋
        IQueryable<MeProduct> GetMeProductsByName(string name);

        //
        int GetMeProductIdByName(string name);

        //
        bool CheckNameRepeat(string name);

        //更新
        bool UpdateMeProductStock(int id);

        //新增
        bool AddMeProduct(MeProduct meProduct);
        //更新
        bool UpdateMeProduct(MeProduct meProduct);

        //刪除
        bool DeleteMeProduct(int id, string name);

    }
}
