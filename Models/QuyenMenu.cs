using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemReportMVC.Models
{
    public partial class QuyenMenu
    {
        [Key]
        public int Id { get; set; }
        public int? QuyenId { get; set; }
        public int? MenuId { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(MenuId))]
        [InverseProperty("QuyenMenu")]
        public virtual Menu Menu { get; set; }
        [ForeignKey(nameof(QuyenId))]
        [InverseProperty("QuyenMenu")]
        public virtual Quyen Quyen { get; set; }
    }
}
