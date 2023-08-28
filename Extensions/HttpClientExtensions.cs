using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Exchange.WebServices.Autodiscover;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Exchange.WebServices.NETStandard.Interfaces;
using Microsoft.Exchange.WebServices.NETStandard.Core;

namespace Microsoft.Exchange.WebServices.NETStandard.Extensions
{
    public static class HttpClientExtensions
    {
        internal static HttpClientHandler CreateHttpClientHandler(string httpClientName)
        {
            switch (httpClientName)
            {
                case ExchangeService.HttpClientName:
                    {
                        return new HttpClientHandler()
                        {
                            UseCookies = false,
                            AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip,
                            AllowAutoRedirect = true
                        };
                    }

                case AutodiscoverService.HttpClientName:
                    {
                        return new HttpClientHandler()
                        {
                            UseCookies = false,
                            AllowAutoRedirect = false
                        };
                    }

                default: 
                    throw new NotSupportedException($"{httpClientName} is not supported!");
            }
        }

        public static IServiceCollection AddEwsServices(this IServiceCollection services)
        {
            services.AddHttpClient(ExchangeService.HttpClientName)
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return CreateHttpClientHandler(ExchangeService.HttpClientName);
                });

            services.AddHttpClient(AutodiscoverService.HttpClientName)
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return CreateHttpClientHandler(AutodiscoverService.HttpClientName);
                });

            services.AddSingleton<IEwsServiceFactory, EwsServiceFactory>();

            return services;
        }
    }
}
