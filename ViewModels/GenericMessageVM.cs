namespace SystemReportMVC.ViewModels
{
    public class GenericMessageVM
    {
        public GenericMessageVM()
        {
            MessageType = GenericMessage.info;
        }
        public bool Status { get; set; }
        public string Message { get; set; }
        public GenericMessage MessageType { get; set; }
        public dynamic Data { get; set; }
    }

    public enum GenericMessage
    {
        warning,
        danger,
        success,
        info,
        error
    }
}