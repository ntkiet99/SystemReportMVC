using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemReportMVC.Models
{
    public partial class DuLieuMauBieu
    {
        [Key]
        public int Id { get; set; }
        [StringLength(150)]
        public string MauBieuId { get; set; }
        [StringLength(150)]
        public string DonViId { get; set; }
        public int? ChiTieuThuocTinhId { get; set; }
        public int? TrangThai { get; set; }
        [StringLength(150)]
        public string Value { get; set; }
        [StringLength(150)]
        public string KyBaoCaoId { get; set; }
    }
}
