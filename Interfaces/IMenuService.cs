using System.Collections.Generic;
using SystemReportMVC.Models;

namespace SystemReportMVC.Interfaces
{
    public interface IMenuService
    {
        IEnumerable<Menu> GetList();
        IEnumerable<Menu> GetParents();     
        Menu GetById(int id);
        void Create(Menu model);
        void Update(Menu model);
        void Delete(int id);
    }
}