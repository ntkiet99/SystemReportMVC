using System.Collections.Generic;
using SystemReportMVC.Helpers;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Interfaces
{
    public interface IUserService
    {
        NguoiDung GetById(int id);
        IEnumerable<NguoiDung> GetAll();
        void Create(NguoiDung model);
        void Update(NguoiDung model);
        void Delete(int id);
        IEnumerable<NguoiDung> GetNguoiDungChuaCoDonVi();
        IEnumerable<NguoiDung> GetNguoiDungTheoDonVi(out int totalRecords, NguoiDungParam nguoiDungParam);
        void AddNguoiDungVaoDonVi(NguoiDung model);
        void DeleteNguoiDungRaKhoiDonVi(NguoiDung model);

        IEnumerable<NguoiDungVM> GetNguoiDungTree();
    }
}
