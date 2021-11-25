using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Models
{
    public partial class DonVi
    {
        public DonVi()
        {
            DonViCons = new HashSet<DonVi>();
            NguoiDung = new HashSet<NguoiDung>();
        }

        [Key]
        [StringLength(150)]
        public string Id { get; set; }
        [StringLength(150)]
        public string Ten { get; set; }
        [StringLength(150)]
        public string TenVietTat { get; set; }
        public string DonViChaId { get; set; }
        public int? Level { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditTs { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(DonViChaId))]
        [InverseProperty(nameof(DonVi.DonViCons))]
        public virtual DonVi DonViCha { get; set; }
        [InverseProperty(nameof(DonVi.DonViCha))]
        public virtual ICollection<DonVi> DonViCons { get; set; }
        [InverseProperty("DonVi")]
        public virtual ICollection<NguoiDung> NguoiDung { get; set; }

        [NotMapped]
        public int RowNum { get; set; }

        public List<DonViVM> GetAllChildrens(ICollection<DonVi> dvc)
        {
            return dvc.Where(x => x.IsDeleted != true).OrderBy(x => x.AuditTs).Select(x => new DonViVM() { 
                id = x.Id,
                text = x.Ten,
                type = "text",
                state = new StateVM() { opened = false},
                children = GetAllChildrens(x.DonViCons)
            }).ToList();
        }
    }
}
