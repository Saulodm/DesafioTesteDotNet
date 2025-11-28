using Microsoft.Extensions.Logging;

namespace Core.ServiceBus
{
    /// <summary>
    /// Implementação muito simples de um bus em memória.
    /// Mantém handlers e publica para todos assinantes. Thread-safe.
    /// </summary>
    public class InMemoryServiceBus<T> : IServiceBus<T>
    {
        private readonly List<Func<T, Task>> _handlers = new();
        private readonly object _lock = new();
        private readonly ILogger<InMemoryServiceBus<T>>? _logger;

        public InMemoryServiceBus(ILogger<InMemoryServiceBus<T>>? logger = null)
        {
            _logger = logger;
        }

        public Task SubscribeAsync(Func<T, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            lock (_lock)
            {
                _handlers.Add(handler);
            }
            _logger?.LogDebug("Handler subscribed for message type {Type}", typeof(T).Name);
            return Task.CompletedTask;
        }

        public async Task PublishAsync(T message)
        {
            List<Func<T, Task>> snapshot;
            lock (_lock)
            {
                snapshot = new List<Func<T, Task>>(_handlers);
            }

            _logger?.LogInformation("Publishing message of type {Type} to {Count} handlers", typeof(T).Name, snapshot.Count);

            foreach (var h in snapshot)
            {
                try
                {
                    await h(message).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Handler threw when processing message of type {Type}", typeof(T).Name);
                }
            }
        }
    }
}
