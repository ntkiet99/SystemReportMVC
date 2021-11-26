using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemReportMVC.Models
{
    public class AuditEntry
    {
        public string TableName { get; set; }
        public string KeyValues { get; set; }
        public string EventTable { get; set; }

        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();

    }
}