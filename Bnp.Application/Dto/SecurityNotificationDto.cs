
using System.Net;

namespace Bnp.Application.Dto
{
    public class SecurityNotificationDto
    {
        public string IsinCode { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public List<string> Messages { get; set; }
    }
}
