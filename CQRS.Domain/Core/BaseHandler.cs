using System.Net;

namespace CQRS.Domain.Core
{
    public class BaseHandler
    {
        private HttpStatusCode _statusCode = HttpStatusCode.OK;
        private readonly List<string> _notifications = new List<string>();

        protected void SetSatusCode(HttpStatusCode statusCode) => _statusCode = statusCode;

        protected void AddNotification(params string[] notifications) => _notifications.AddRange(notifications);

        public List<string> GetNotifications() => _notifications;

        public HttpStatusCode GetStatusCode() => _statusCode;
    }
}
