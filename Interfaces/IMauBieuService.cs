using System.Collections.Generic;
using SystemReportMVC.Helpers;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Interfaces
{
    public interface IMauBieuService
    {
        IEnumerable<MauBieu> Get();
        MauBieu GetById(string id);
        void Create(MauBieu model);
        void Update(MauBieu model);
        void Delete(string id);
        List<MauBieuVM> GetTreeJS();
        void GenerateChiTieuThuocTinh(string mauBieuId);
        RenderMauBieuVM RenderMauBieu(string mauBieuId);
        void PhanQuyenBaoCao(string mauBieuId, string donViId);
        List<DonVi> GetDonViByMauBieuId(string mauBieuId);
        void DeleteDonViInMauBieu(string mauBieuId, string donViId);

        List<MauBieu> ListMauBaoCaoByDonViId(string donViId);
        RenderMauBieuVM RenderMauBieuNhapLieu(AppUser user, string mauBieuId);
        void SaveData(List<InputFormVM> inputs);
        void TrangThaiNhapLieu(string mauBieuId, string donViId);
        void TrangThaiDuyet(string mauBieuId, string donViId);
        void TrangThaiXuatBan(string mauBieuId, string donViId);
        List<DuLieuMauBieu> GetDuLieuMau(string donViId);
    }
}