using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FamilyHealthCare.SharedLibrary
{
    public static class HttpClientRegister
    {
        public static void AddCustomHttpClient(this IServiceCollection services, IConfiguration config)
        {
            var configClient = new Action<IServiceProvider, HttpClient>(async (provider, client) =>
            {
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync(RequestConstants.ACCESS_TOKEN);
                client.BaseAddress = new Uri(config[RequestConstants.BACK_END_ENDPOINT]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(RequestConstants.BEARER, accessToken);
            });

            var configNotificaitonClient = new Action<IServiceProvider, HttpClient>(async (provider, client) =>
            {
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync(RequestConstants.ACCESS_TOKEN);

                client.BaseAddress = new Uri(config[RequestConstants.NOTIFICATION_ENDPOINT]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(RequestConstants.BEARER, accessToken);
            });

            services.AddHttpClient(ServiceConstants.BACK_END_NAMED_CLIENT, configClient);
            services.AddHttpClient(ServiceConstants.NOTIFICATION_NAMED_CLIENT, configNotificaitonClient);
        }
    }
}
