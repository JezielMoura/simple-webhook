using System.Collections.Generic;
using webhook.Models;

namespace webhook.Services
{
    public interface IWebHookStorage
    {
        void LogRequest(string id, RequestLog log);
        List<RequestLog> GetRequests(string id);
        string GenerateId();
    }
}