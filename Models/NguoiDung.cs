using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemReportMVC.Models
{
    public partial class NguoiDung
    {
        public NguoiDung()
        {
            NguoiDungQuyen = new HashSet<NguoiDungQuyen>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(150)]
        public string TaiKhoan { get; set; }
        [StringLength(150)]
        public string MatKhau { get; set; }
        [StringLength(150)]
        public string HoTen { get; set; }
        [StringLength(50)]
        public string SoDienThoai { get; set; }
        [StringLength(150)]
        public string Email { get; set; }
        [StringLength(500)]
        public string HinhAnh { get; set; }
        public int? FileId { get; set; }
        [StringLength(150)]
        public string DonViId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuditTs { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(DonViId))]
        [InverseProperty("NguoiDung")]
        public virtual DonVi DonVi { get; set; }
        [InverseProperty("NguoiDung")]
        public virtual ICollection<NguoiDungQuyen> NguoiDungQuyen { get; set; }

        [NotMapped]
        public int RowNum { get; set; }
    }
}
