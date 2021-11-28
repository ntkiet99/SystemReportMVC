using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemReportMVC.Models
{
    public partial class ChiTieuThuocTinh
    {
        [Key]
        public int Id { get; set; }
        [StringLength(150)]
        public string ChiTieuId { get; set; }
        [StringLength(150)]
        public string ThuocTinhId { get; set; }
        [StringLength(150)]
        public string MauBieuId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditTs { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
