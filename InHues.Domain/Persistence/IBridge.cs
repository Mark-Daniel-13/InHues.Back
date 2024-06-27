using InHues.Domain.DTO.Custom;
using InHues.Domain.Enums;
using System.Threading.Tasks;

namespace InHues.Domain.Persistence
{
    public interface IBridge
    {
        public Task<HttpResponse<TResponse>> GetJsonAsync<TResponse>(string url, OdataRequestBase requestParams);
        public Task<HttpResponse<TResponse>> GetJsonAsync<TResponse>(string url);
        public Task<HttpResponse<TResponse>> CommandJsonAsync<TResponse, TRequest>(string url, TRequest request, RequestType requesType);
    }
}
