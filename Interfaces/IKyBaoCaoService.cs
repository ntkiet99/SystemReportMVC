using System.Collections.Generic;
using SystemReportMVC.Models;

namespace SystemReportMVC.Interfaces
{
    public interface IKyBaoCaoService
    {
        IEnumerable<KyBaoCao> GetList(int nam);
        KyBaoCao GetById(int id);
        void Create(KyBaoCao model);
        void Update(KyBaoCao model);
        void Delete(int id);
    }
}
