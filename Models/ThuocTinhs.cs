using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemReportMVC.Models
{
    public partial class ThuocTinhs
    {
        public ThuocTinhs()
        {
            ThuocTinhCons = new HashSet<ThuocTinhs>();
        }

        [Key]
        [StringLength(150)]
        public string Id { get; set; }
        public int? IdInt { get; set; }
        [StringLength(150)]
        public string TenThuocTinh { get; set; }
        [StringLength(150)]
        public string NhanThuocTinh { get; set; }
        public int? ThuTu { get; set; }
        public int? Level { get; set; }
        [StringLength(150)]
        public string ThuocTinhChaId { get; set; }
        public int? KieuDuLieu { get; set; }
        public int? MauBieuId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditTs { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(MauBieuId))]
        [InverseProperty(nameof(MauBieus.ThuocTinhs))]
        public virtual MauBieus MauBieu { get; set; }
        [ForeignKey(nameof(ThuocTinhChaId))]
        [InverseProperty(nameof(ThuocTinhs.ThuocTinhCons))]
        public virtual ThuocTinhs ThuocTinhCha { get; set; }
        [InverseProperty(nameof(ThuocTinhs.ThuocTinhCha))]
        public virtual ICollection<ThuocTinhs> ThuocTinhCons { get; set; }
    }
}
