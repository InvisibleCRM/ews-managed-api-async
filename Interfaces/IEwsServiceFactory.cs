using System;
using Microsoft.Exchange.WebServices.Data;

namespace Microsoft.Exchange.WebServices.NETStandard.Interfaces
{
    public interface IEwsServiceFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeService"/> class, targeting
        /// the latest supported version of EWS and scoped to the system's current time zone.
        /// </summary>
        public ExchangeService CreateService();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeService"/> class, targeting
        /// the latest supported version of EWS and scoped to the specified time zone.
        /// </summary>
        /// <param name="timeZone">The time zone to which the service is scoped.</param>
        public ExchangeService CreateService(TimeZoneInfo timeZone);

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeService"/> class, targeting
        /// the specified version of EWS and scoped to the system's current time zone.
        /// </summary>
        /// <param name="requestedServerVersion">The version of EWS that the service targets.</param>
        public ExchangeService CreateService(ExchangeVersion requestedServerVersion);

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeService"/> class, targeting
        /// the specified version of EWS and scoped to the specified time zone.
        /// </summary>
        /// <param name="requestedServerVersion">The version of EWS that the service targets.</param>
        /// <param name="timeZone">The time zone to which the service is scoped.</param>
        public ExchangeService CreateService(ExchangeVersion requestedServerVersion, TimeZoneInfo timeZone);
    }
}
