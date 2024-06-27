using System.Net;

namespace InHues.Domain.DTO.Custom
{
    public class HttpResponse<T>
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public T? Response { get; set; }
        public string[] Errors { get; set; }
    }
}
