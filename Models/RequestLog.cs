using System;
using System.Collections.Generic;

namespace webhook.Models
{
    public class RequestLog
    {
        public string Method { get; set; } = string.Empty;
        public Dictionary<string, string> Headers { get; set; } = new();
        public string Body { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; } = string.Empty;
    }
}