using System.Collections.Generic;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Interfaces
{
    public interface IAuditLogService
    {
        IEnumerable<AuditLogVM> GetList();
    }
}
