using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemReportMVC.Models
{
    public partial class Quyen
    {
        public Quyen()
        {
            NguoiDungQuyen = new HashSet<NguoiDungQuyen>();
            QuyenMenu = new HashSet<QuyenMenu>();
        }

        [Key]
        public int Id { get; set; }
        public int? Code { get; set; }
        [StringLength(150)]
        public string Ten { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditTs { get; set; }
        public bool? IsDeleted { get; set; }

        [InverseProperty("Quyen")]
        public virtual ICollection<NguoiDungQuyen> NguoiDungQuyen { get; set; }
        [InverseProperty("Quyen")]
        public virtual ICollection<QuyenMenu> QuyenMenu { get; set; }
        [NotMapped]
        public List<Menu> Menus { get; set; } = new List<Menu>();
        [NotMapped]
        public List<int> MenuIds { get; set; } = new List<int>();
    }
}
