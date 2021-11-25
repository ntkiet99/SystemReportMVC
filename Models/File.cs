using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemReportMVC.Models
{
    public partial class File
    {
        [Key]
        public int Id { get; set; }
        [StringLength(500)]
        public string FileName { get; set; }
        [StringLength(500)]
        public string SaveName { get; set; }
        [StringLength(500)]
        public string Path { get; set; }
        public long? Size { get; set; }
        [StringLength(500)]
        public string Ext { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditTs { get; set; }
        public int? AuditUserId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
