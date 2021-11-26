using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemReportMVC.ViewModels
{
    public class NguoiDungVM
    {
        public string id { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public StateVM state { get; set; }


    }
}