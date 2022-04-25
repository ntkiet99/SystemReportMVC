using System.Collections.Generic;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Interfaces
{
    public interface IThuocTinhService
    {
        IEnumerable<ThuocTinh> Get();
        ThuocTinh GetById(string id);
        void Create(ThuocTinh model);
        void Update(ThuocTinh model);
        void Delete(string id);
        List<ThuocTinhVM> GetTreeJS(string bieuMauId);
        List<RenderThuocTinhVM> GetListOrderByLevel(string bieuMauId);
        List<ThuocTinh> GetListMaxLevel(string bieuMauId);
        void NhapLieuThuocTinh(List<ThuocTinhNhapLieu> list);
    }
}
