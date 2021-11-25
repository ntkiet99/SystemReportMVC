using System.Collections.Generic;
using SystemReportMVC.Models;

namespace SystemReportMVC.Interfaces
{
    public interface IRoleService
    {
        Quyen GetById(int id);
        IEnumerable<Quyen> GetList();
        void Create(Quyen model);
        void Delete(int id);
    }
}