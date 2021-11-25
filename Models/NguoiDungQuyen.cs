using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemReportMVC.Models
{
    public partial class NguoiDungQuyen
    {
        [Key]
        public int Id { get; set; }
        public int? NguoiDungId { get; set; }
        public int? QuyenId { get; set; }

        [ForeignKey(nameof(NguoiDungId))]
        [InverseProperty("NguoiDungQuyen")]
        public virtual NguoiDung NguoiDung { get; set; }
        [ForeignKey(nameof(QuyenId))]
        [InverseProperty("NguoiDungQuyen")]
        public virtual Quyen Quyen { get; set; }
    }
}
