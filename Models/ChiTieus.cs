using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemReportMVC.Models
{
    public partial class ChiTieus
    {
        public ChiTieus()
        {
            ChiTieuCons = new HashSet<ChiTieus>();
        }

        [Key]
        [StringLength(150)]
        public string Id { get; set; }
        public int? IdInt { get; set; }
        [StringLength(150)]
        public string TenChiTieu { get; set; }
        [StringLength(150)]
        public string NhanChiTieu { get; set; }
        public int? ThuTu { get; set; }
        public int? Level { get; set; }
        public int? KieuDuLieu { get; set; }
        public int? MauBieuId { get; set; }
        [StringLength(150)]
        public string ChiTieuChaId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditTs { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(ChiTieuChaId))]
        [InverseProperty(nameof(ChiTieus.ChiTieuCons))]
        public virtual ChiTieus ChiTieuCha { get; set; }
        [ForeignKey(nameof(MauBieuId))]
        [InverseProperty(nameof(MauBieus.ChiTieus))]
        public virtual MauBieus MauBieu { get; set; }
        [InverseProperty(nameof(ChiTieus.ChiTieuCha))]
        public virtual ICollection<ChiTieus> ChiTieuCons { get; set; }
    }
}
