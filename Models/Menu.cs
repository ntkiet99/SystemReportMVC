using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemReportMVC.Models
{
    public partial class Menu
    {
        public Menu()
        {
            MenuCons = new HashSet<Menu>();
            QuyenMenu = new HashSet<QuyenMenu>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(150)]
        public string Ten { get; set; }
        [StringLength(150)]
        public string Path { get; set; }
        [StringLength(150)]
        public string Icon { get; set; }
        public int? Level { get; set; }
        public int? ThuTuHienThi { get; set; }
        public int? MenuChaId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditTs { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(MenuChaId))]
        [InverseProperty(nameof(Menu.MenuCons))]
        public virtual Menu MenuCha { get; set; }
        [InverseProperty(nameof(Menu.MenuCha))]
        public virtual ICollection<Menu> MenuCons { get; set; }
        [InverseProperty("Menu")]
        public virtual ICollection<QuyenMenu> QuyenMenu { get; set; }

        [NotMapped]
        public bool Active { get; set; } = false;
    }
}
