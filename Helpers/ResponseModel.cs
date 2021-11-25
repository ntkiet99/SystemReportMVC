namespace SystemReportMVC.Helpers
{
    public class ResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}