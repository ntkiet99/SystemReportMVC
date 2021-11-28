using System.Collections.Generic;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Interfaces
{
    public interface IChiTieuService
    {
        IEnumerable<ChiTieu> Get();
        ChiTieu GetById(string id);
        void Create(ChiTieu model);
        void Update(ChiTieu model);
        void Delete(string id);
        List<ChiTieuVM> GetTreeJS(string mauBieuId);
    }
}
