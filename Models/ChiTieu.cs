using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Models
{
    public partial class ChiTieu
    {
        public ChiTieu()
        {
            ChiTieuCons = new HashSet<ChiTieu>();
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
        [StringLength(150)]
        public string MauBieuId { get; set; }
        [StringLength(150)]
        public string ChiTieuChaId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditTs { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(ChiTieuChaId))]
        [InverseProperty(nameof(ChiTieu.ChiTieuCons))]
        public virtual ChiTieu ChiTieuCha { get; set; }
        [ForeignKey(nameof(MauBieuId))]
        [InverseProperty(nameof(Models.MauBieu.ChiTieus))]
        public virtual MauBieu MauBieu { get; set; }
        [InverseProperty(nameof(ChiTieu.ChiTieuCha))]
        public virtual ICollection<ChiTieu> ChiTieuCons { get; set; }
        public List<ChiTieuVM> GetAllChildrens(ICollection<ChiTieu> dvc)
        {
            return dvc.Where(x => x.IsDeleted != true).OrderBy(x => x.AuditTs).Select(x => new ChiTieuVM()
            {
                id = x.Id,
                text = x.TenChiTieu,
                type = "text",
                state = new StateVM() { opened = false },
                children = GetAllChildrens(x.ChiTieuCons)
            }).ToList();
        }
    }
}
