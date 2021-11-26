using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemReportMVC.Models
{
    public partial class HistoryData
    {
        public Guid? HistoryDataId { get; set; }
        public string HistoryDataCode { get; set; }
        public string HistoryDataName { get; set; }
        public Guid? ActionId { get; set; }
        public Guid? TraceId { get; set; }
        public string Action { get; set; }
        public string EventTable { get; set; }
        public string TableName { get; set; }
        public string PrimaryKey { get; set; }
        public string ColumnName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string RefValue { get; set; }
        public int? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        public bool? Deleted { get; set; }
    }
}
