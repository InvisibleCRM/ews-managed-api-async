using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Exchange.WebServices.NETStandard.Interfaces;

namespace Microsoft.Exchange.WebServices.NETStandard.Core
{
    internal class EwsServiceFactory : IEwsServiceFactory
    {
        private readonly IHttpMessageHandlerFactory httpMessageHandlerFactory;

        public EwsServiceFactory(IHttpMessageHandlerFactory httpMessageHandlerFactory) 
        {
            this.httpMessageHandlerFactory = httpMessageHandlerFactory;
        }

        public ExchangeService CreateService()
        {
            return new ExchangeService(httpMessageHandlerFactory);
        }

        public ExchangeService CreateService(TimeZoneInfo timeZone)
        {
            return new ExchangeService(timeZone, httpMessageHandlerFactory);
        }

        public ExchangeService CreateService(ExchangeVersion requestedServerVersion)
        {
            return new ExchangeService(requestedServerVersion, httpMessageHandlerFactory);
        }

        public ExchangeService CreateService(ExchangeVersion requestedServerVersion, TimeZoneInfo timeZone)
        {
            return new ExchangeService(requestedServerVersion, timeZone, httpMessageHandlerFactory);
        }
    }
}
