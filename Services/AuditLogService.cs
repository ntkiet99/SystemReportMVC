using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemReportMVC.Data;
using SystemReportMVC.Interfaces;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Services
{
    public class AuditLogService : IAuditLogService
    {
        private DataContext _context;
        public AuditLogService(DataContext context)
        {
            _context = context;
        }
        public IEnumerable<AuditLogVM> GetList()
        {
            var auditlogs = _context.HistoryDatas.Where(x => x.Deleted != true).OrderByDescending(x => x.CreatedAt).ToList();
            var users = _context.NguoiDungs.Where(x => x.IsDeleted != true).ToList();
            var result = auditlogs.Select(x => new AuditLogVM()
            {
                Id = x.HistoryDataId.ToString(),
                EventTable = x.EventTable,
                TableName = x.TableName,
                Action = x.Action,
                OldValue = x.OldValue,
                NewValue = x.NewValue,
                UserId = x.UserId,
                UserName = users.Where(b => x.UserId == b.Id && x.Deleted != true).FirstOrDefault().TaiKhoan,
                FullName = users.Where(b => x.UserId == b.Id && x.Deleted != true).FirstOrDefault().HoTen,
                AuditTime = x.CreatedAt.Value,
            }).ToList();
            return result;
        }
    }
}