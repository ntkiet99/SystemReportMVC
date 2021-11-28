using System.Collections.Generic;

namespace SystemReportMVC.ViewModels
{
    public class MauBieuVM
    {
        public string id { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public StateVM state { get; set; }
        public List<MauBieuVM> children { get; set; } = new List<MauBieuVM>();
    }

    public class RenderMauBieuVM
    {
        public string MauBieuId { get; set; }
        public string TenMauBieu { get; set; }
        public string DonViNhan { get; set; }
        public string DonViGui { get; set; }
        public string KyHieu { get; set; }
        public string GhiChu { get; set; }
        public List<RenderThuocTinhVM> RenderThuocTinh { get; set; }
        public List<RenderChiTieuVM> RenderChiTieu { get; set; }
    }
}