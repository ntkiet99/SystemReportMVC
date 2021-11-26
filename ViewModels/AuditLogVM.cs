using System;

namespace SystemReportMVC.ViewModels
{
    public class AuditLogVM
    {
        public string Id { get; set; }
        public string EventTable { get; set; }
        public string TableName { get; set; }
        public string Action { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime AuditTime { get; set; }
    }
}