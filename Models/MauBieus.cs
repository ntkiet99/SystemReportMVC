using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemReportMVC.Models
{
    public partial class MauBieus
    {
        public MauBieus()
        {
            ChiTieus = new HashSet<ChiTieus>();
            MauBieuCons = new HashSet<MauBieus>();
            ThuocTinhs = new HashSet<ThuocTinhs>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(500)]
        public string Ten { get; set; }
        [StringLength(100)]
        public string KyHieu { get; set; }
        public int? Stt { get; set; }
        [StringLength(500)]
        public string GhiChu { get; set; }
        [StringLength(50)]
        public string DonViNhanId { get; set; }
        [StringLength(50)]
        public string DonViGiaoId { get; set; }
        public int? NguoiTaoId { get; set; }
        public int? NguoiCapNhatId { get; set; }
        public int? NhomMauBieu { get; set; }
        public int? MauBieuChaId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditTs { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(MauBieuChaId))]
        [InverseProperty(nameof(MauBieus.MauBieuCons))]
        public virtual MauBieus MauBieuCha { get; set; }
        [InverseProperty("MauBieu")]
        public virtual ICollection<ChiTieus> ChiTieus { get; set; }
        [InverseProperty(nameof(MauBieus.MauBieuCha))]
        public virtual ICollection<MauBieus> MauBieuCons { get; set; }
        [InverseProperty("MauBieu")]
        public virtual ICollection<ThuocTinhs> ThuocTinhs { get; set; }
    }
}
