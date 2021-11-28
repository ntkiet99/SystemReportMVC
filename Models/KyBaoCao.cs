using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemReportMVC.Models
{
    public partial class KyBaoCao
    {
        [Key]
        public int Id { get; set; }
        public int? Nam { get; set; }
        [StringLength(200)]
        public string Ten { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? TuNgay { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DenNgay { get; set; }
        public int? TinhTrang { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? NgayGui { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? NgayBanHanh { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditTs { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
