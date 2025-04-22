using System.Net.Http.Headers;

namespace Client_Laia_T1.PR2.APIrest.Services
{
    public class AthenticationUtils: DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AthenticationUtils(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["authenitcationToken"];

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
