using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE_ManagementSystem.Models.Repo
{
    interface IProductRepo
    {
        //列出所以Product清單
        IQueryable<Product> ListAllProduct();

        //列出所以Product清單
        IQueryable<Product> ListAllProductUpdateDue();

        //列出所以Product清單
        Product LinkToResume(string id);

        //列出某個Product紀錄
        Product GetProductById(int id);

        //搜尋
        IQueryable<Product> GetProductsByName(string name);

        //新增
        bool AddProduct(Product product);
        //更新
        bool UpdateProduct(Product product);

        //刪除
        bool DeleteProduct(Product product);

    }
}
