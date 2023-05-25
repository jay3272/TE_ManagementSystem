using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE_ManagementSystem.Models.Repo
{
    interface IPORepo
    {
        //列出所以Product Transaction清單
        List<ViewProductTransaction> ListAllProductTransaction();

        //列出某個Product Transaction紀錄
        IQueryable<ProductTransaction> GetProductTransactionById(string id);

        //搜尋
        IQueryable<ProductTransaction> GetProductsTransactionByName(string name);

        //新增
        bool AddProductTransaction(ProductTransaction productTransaction);
        //更新
        bool UpdateProductTransaction(ProductTransaction productTransaction);

        //刪除
        bool DeleteProductTransaction(ProductTransaction productTransaction);

    }
}
