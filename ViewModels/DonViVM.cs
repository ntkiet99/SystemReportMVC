using System.Collections.Generic;

namespace SystemReportMVC.ViewModels
{
    public class DonViVM
    {
        public string id { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public StateVM state { get; set; }
        public List<DonViVM> children { get; set; } = new List<DonViVM>();
    }
}