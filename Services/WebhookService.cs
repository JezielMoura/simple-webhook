using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webhook.Models;

namespace Webhook.Services
{
    public class WebhookService
    {
        private readonly Dictionary<string, List<WebhookRequest>> _webhookRequests = new();
        private readonly object _lockObject = new object();

        public string CreateWebhookUrl()
        {
            var webhookId = Guid.NewGuid().ToString("N");
            lock (_lockObject)
            {
                if (!_webhookRequests.ContainsKey(webhookId))
                {
                    _webhookRequests[webhookId] = new List<WebhookRequest>();
                }
            }
            return webhookId;
        }

        public void AddRequest(string webhookId, WebhookRequest request)
        {
            lock (_lockObject)
            {
                if (_webhookRequests.ContainsKey(webhookId))
                {
                    _webhookRequests[webhookId].Add(request);
                }
            }
        }

        public List<WebhookRequest> GetRequests(string webhookId)
        {
            lock (_lockObject)
            {
                return _webhookRequests.ContainsKey(webhookId) 
                    ? _webhookRequests[webhookId].OrderByDescending(r => r.Timestamp).ToList()
                    : new List<WebhookRequest>();
            }
        }

        public void ClearRequests(string webhookId)
        {
            lock (_lockObject)
            {
                if (_webhookRequests.ContainsKey(webhookId))
                {
                    _webhookRequests[webhookId].Clear();
                }
            }
        }

        public bool WebhookExists(string webhookId)
        {
            lock (_lockObject)
            {
                return _webhookRequests.ContainsKey(webhookId);
            }
        }

        public void DeleteWebhook(string webhookId)
        {
            lock (_lockObject)
            {
                _webhookRequests.Remove(webhookId);
            }
        }

        public List<string> GetAllWebhookIds()
        {
            lock (_lockObject)
            {
                return _webhookRequests.Keys.ToList();
            }
        }
    }
}