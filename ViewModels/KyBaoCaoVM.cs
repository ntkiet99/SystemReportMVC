using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemReportMVC.ViewModels
{
    public class KyBaoCaoVM
    {
        public int Id { get; set; }
        public int? Nam { get; set; }
        public string Ten { get; set; }
        public string TuNgay { get; set; }
        public string DenNgay { get; set; }
        public int? TinhTrang { get; set; }
        public string NgayGui { get; set; }
        public string NgayBanHanh { get; set; }
        public string CreateAt { get; set; }
        public string AuditTs { get; set; }
        public bool? IsDeleted { get; set; }
    }
}