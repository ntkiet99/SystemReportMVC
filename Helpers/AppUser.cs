using System.Collections.Generic;

namespace SystemReportMVC.Helpers
{
    public class AppUser
    {
        public int Id { get; set; }
        public string TaiKhoan { get; set; }
        public string HoTen { get; set; }
        public string HinhAnh { get; set; }
        public List<int> RoleCodes { get; set; } = new List<int>();
        public string DonViId { get; set; }
    }
}