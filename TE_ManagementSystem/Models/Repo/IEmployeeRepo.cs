using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE_ManagementSystem.Models.Repo
{
    interface IEmployeeRepo
    {
        //列出清單
        IQueryable<Employee> ListAllEmployee();

        //列出某個紀錄
        Employee GetEmployeeById(int id);

        //搜尋
        Employee GetEmployeeByName(string name);

        //新增
        bool AddEmployee(Employee employee);
        //更新
        bool UpdateEmployee(Employee employee);

        //刪除
        bool DeleteEmployee(Employee employee);

    }
}
