using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Models
{
    public partial class ThuocTinh
    {
        public ThuocTinh()
        {
            ThuocTinhCons = new HashSet<ThuocTinh>();
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
        public int? Row { get; set; } = 0;
        public int? Col { get; set; } = 0;
        public int? Width { get; set; } = 0;
        public int? Level { get; set; }
        [StringLength(150)]
        public string ThuocTinhChaId { get; set; }
        public int? KieuDuLieu { get; set; }
        [StringLength(150)]
        public string MauBieuId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditTs { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(MauBieuId))]
        [InverseProperty(nameof(Models.MauBieu.ThuocTinhs))]
        public virtual MauBieu MauBieu { get; set; }
        [ForeignKey(nameof(ThuocTinhChaId))]
        [InverseProperty(nameof(ThuocTinh.ThuocTinhCons))]
        public virtual ThuocTinh ThuocTinhCha { get; set; }
        [InverseProperty(nameof(ThuocTinh.ThuocTinhCha))]
        public virtual ICollection<ThuocTinh> ThuocTinhCons { get; set; }
        public List<ThuocTinhVM> GetAllChildrens(ICollection<ThuocTinh> dvc)
        {
            return dvc.Where(x => x.IsDeleted != true).OrderBy(x => x.AuditTs).Select(x => new ThuocTinhVM()
            {
                id = x.Id,
                text = x.TenThuocTinh,
                type = "text",
                state = new StateVM() { opened = false },
                children = GetAllChildrens(x.ThuocTinhCons)
            }).ToList();
        }
    }
}
