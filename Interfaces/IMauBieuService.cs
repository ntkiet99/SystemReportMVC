using System.Collections.Generic;
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
    }
}