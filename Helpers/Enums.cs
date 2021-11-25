using System.ComponentModel.DataAnnotations;

namespace SystemReportMVC.Helpers
{
    public enum ERoleCodes
    {
        [Display(Name = "Quản trị viên")]
        QuanTriVien = 9999,
        [Display(Name = "Lãnh đạo")]
        LanhDao = 8888,
        [Display(Name = "Tổng hợp báo cáo")]
        QuanLyDuAn = 7777,
        [Display(Name = "Duyệt báo cáo")]
        PhongBanChuyenMon = 6666,
        [Display(Name = "Nhập liệu báo cáo")]
        CanBoXuLy = 5555
    }

    public enum EBaoCaos
    {
        [Display(Name = "Khởi tạo")]
        KhoiTao = 0,
        [Display(Name = "Nhập liệu")]
        LanhDao = 1,
        [Display(Name = "Trình lãnh đạo")]
        QuanLyDuAn = 2,
        [Display(Name = "Đã duyệt")]
        PhongBanChuyenMon = 3,
        [Display(Name = "Từ chối")]
        CanBoXuLy = -1
    }
}