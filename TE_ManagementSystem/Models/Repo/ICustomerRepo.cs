using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE_ManagementSystem.Models.Repo
{
    interface ICustomerRepo
    {
        //列出清單
        IQueryable<Customer> ListAllCustomer();

        //列出某個紀錄
        Customer GetCustomerById(int id);

        //搜尋
        Customer GetCustomerByName(string name);

        //新增
        bool AddCustomer(Customer customer);
        //更新
        bool UpdateCustomer(Customer customer);

        //刪除
        bool DeleteCustomer(Customer customer);

    }
}
