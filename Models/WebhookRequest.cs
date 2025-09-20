using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Webhook.Models
{
    public class WebhookRequest
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public string WebhookId { get; set; } = string.Empty;
        
        [Required]
        public string Method { get; set; } = string.Empty;
        
        public string? Path { get; set; }
        
        public string? QueryString { get; set; }
        
        public string? Headers { get; set; }
        
        public string? Body { get; set; }
        
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        public string? ContentType { get; set; }
        
        public string? IpAddress { get; set; }
        
        public string? UserAgent { get; set; }
    }
}