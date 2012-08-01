﻿using System.Collections.Generic;

namespace Chinchilla.Api
{
    public interface IRabbitAdmin
    {
        IEnumerable<VirtualHost> VirtualHosts { get; }

        IEnumerable<Exchange> Exchanges(VirtualHost virtualHost);

        IEnumerable<Queue> Queues(VirtualHost virtualHost);

        bool Create(VirtualHost virtualHost);

        bool Delete(VirtualHost virtualHost);

        bool Create(VirtualHost virtualHost, User user, Permissions permissions);
    }
}
