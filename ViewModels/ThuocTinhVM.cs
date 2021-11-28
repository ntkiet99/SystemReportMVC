using System.Collections.Generic;
using SystemReportMVC.Models;

namespace SystemReportMVC.ViewModels
{
    public class ThuocTinhVM
    {
        public string id { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public StateVM state { get; set; }
        public List<ThuocTinhVM> children { get; set; } = new List<ThuocTinhVM>();
    }

    public class RenderThuocTinhVM
    {
        public int Level { get; set; }
        public List<ThuocTinh> ThuocTinhs { get; set; } = new List<ThuocTinh>();
    }
}