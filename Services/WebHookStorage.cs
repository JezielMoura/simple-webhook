using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using webhook.Models;
using webhook.Services;

namespace webhook.Services
{
    public class WebHookStorage : IWebHookStorage
    {
        private readonly ConcurrentDictionary<string, List<RequestLog>> _storage = new();

        public void LogRequest(string id, RequestLog log)
        {
            if (!_storage.ContainsKey(id))
            {
                _storage[id] = new List<RequestLog>();
            }
            _storage[id].Add(log);
        }

        public List<RequestLog> GetRequests(string id)
        {
            return _storage.GetValueOrDefault(id, new List<RequestLog>());
        }

        public string GenerateId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var id = new StringBuilder(8);
            for (int i = 0; i < 8; i++)
            {
                id.Append(chars[random.Next(chars.Length)]);
            }
            return id.ToString();
        }
    }
}