using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ServiceBus
{
    /// <summary>
    /// Simula um service-bus simples e genérico (in-memory).
    /// </summary>
    public interface IServiceBus<T>
    {
        Task PublishAsync(T message);
        Task SubscribeAsync(Func<T, Task> handler);
    }
}
