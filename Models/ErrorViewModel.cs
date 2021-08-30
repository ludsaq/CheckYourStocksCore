using System;

namespace CheckYourStocks.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public static string ErrorLoadQuik = "";
        public static string ErrorGetData = "";
    }
}
