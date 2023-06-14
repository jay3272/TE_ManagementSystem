using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE_ManagementSystem.Models.Repo
{
    interface ISupplierRepo
    {
        //列出清單
        IQueryable<Supplier> ListAllSupplier();

        //列出某個紀錄
        Supplier GetSupplierById(int id);

        //搜尋
        Supplier GetSupplierByName(string name);

        //新增
        bool AddSupplier(Supplier supplier);
        //更新
        bool UpdateSupplier(Supplier supplier);

        //刪除
        bool DeleteSupplier(int id, string name);

    }
}
