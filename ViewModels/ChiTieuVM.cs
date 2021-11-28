using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemReportMVC.ViewModels
{
    public class ChiTieuVM 
    {
        public string id { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public StateVM state { get; set; }
        public List<ChiTieuVM> children { get; set; } = new List<ChiTieuVM>();
    }

    public class RenderChiTieuVM
    {
        public string TenChiTieu { get; set; }
        public int ThuTu { get; set; }
        public List<RenderInputVM> Inputs { get; set; } = new List<RenderInputVM>();
    }

    public class RenderInputVM
    {
        public int Id { get; set; }
        public int TypeValue { get; set; }
        public int TypeInput { get; set; }
        public string Value { get; set; }
    }
}