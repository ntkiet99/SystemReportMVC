using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Models
{
    public partial class MauBieu
    {
        public MauBieu()
        {
            ChiTieus = new HashSet<ChiTieu>();
            MauBieuCons = new HashSet<MauBieu>();
            ThuocTinhs = new HashSet<ThuocTinh>();
        }

        [Key]
        [StringLength(150)]
        public string Id { get; set; }
        [StringLength(500)]
        public string Ten { get; set; }
        [StringLength(100)]
        public string KyHieu { get; set; }
        public int? Stt { get; set; }
        public int? Level { get; set; }
        [StringLength(500)]
        public string GhiChu { get; set; }
        [StringLength(50)]
        public string DonViNhanId { get; set; }
        [StringLength(50)]
        public string DonViGiaoId { get; set; }
        public int? NguoiTaoId { get; set; }
        public int? NguoiCapNhatId { get; set; }
        public int? NhomMauBieu { get; set; }
        public int? ChoPhepNhap { get; set; }
        [StringLength(150)]
        public string MauBieuChaId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditTs { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(MauBieuChaId))]
        [InverseProperty(nameof(MauBieu.MauBieuCons))]
        public virtual MauBieu MauBieuCha { get; set; }
        [InverseProperty("MauBieu")]
        public virtual ICollection<ChiTieu> ChiTieus { get; set; }
        [InverseProperty(nameof(MauBieu.MauBieuCha))]
        public virtual ICollection<MauBieu> MauBieuCons { get; set; }
        [InverseProperty("MauBieu")]
        public virtual ICollection<ThuocTinh> ThuocTinhs { get; set; }
        public List<MauBieuVM> GetAllChildrens(ICollection<MauBieu> dvc)
        {
            return dvc.Where(x => x.IsDeleted != true).OrderBy(x => x.AuditTs).Select(x => new MauBieuVM()
            {
                id = x.Id,
                text = x.Ten,
                type = x.NhomMauBieu == 1? "root": "text",
                state = new StateVM() { opened = false },
                children = GetAllChildrens(x.MauBieuCons)
            }).ToList();
        }
    }
}
