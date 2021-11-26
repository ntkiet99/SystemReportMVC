using System.Collections.Generic;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Interfaces
{
    public interface IRoleService
    {
        Quyen GetById(int id);
        IEnumerable<Quyen> GetList();
        void Create(Quyen model);
        void Delete(int id);

        IEnumerable<Quyen> GetRoleByUserId(int userId);
        void AddQuyenVaoNguoiDung(QuyenNguoiDungVM model);
    }
}