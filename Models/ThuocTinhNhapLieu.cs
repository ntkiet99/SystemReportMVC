using System.ComponentModel.DataAnnotations;

namespace SystemReportMVC.Models
{
    public class ThuocTinhNhapLieu
    {
        [Key]
        public int Id { get; set; }
        [StringLength(150)]
        public string MauBieuId { get; set; }
        [StringLength(150)]
        public string ThuocTinhId { get; set; }
        [StringLength(500)]
        public string Value { get; set; }
        public int? OrderBy { get; set; }
    }
}