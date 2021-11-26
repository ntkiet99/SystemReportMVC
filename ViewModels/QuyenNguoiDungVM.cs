using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemReportMVC.ViewModels
{
    public class QuyenNguoiDungVM
    {
        public int Id { get; set; }
        public List<int> QuyenIds { get; set; } = new List<int>();
    }
}