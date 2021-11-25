using System;

namespace SystemReportMVC.Models
{
    public class Audit
    {
        public int AuditUserId { get; set; } = 0;
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime AuditTs { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}