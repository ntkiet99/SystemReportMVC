using System.Collections.Generic;
using SystemReportMVC.Helpers;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Interfaces
{
    public interface IDonViService
    {
        IEnumerable<DonVi> Get();
        List<DonVi> Pagination(out int totalRecords, DataTableAjaxPostModel datatableParams);
        DonVi GetById(string id);
        void Create(DonVi model);
        void Update(DonVi model);
        void Delete(string id);
        List<DonViVM> GetTreeJS();
    }
}