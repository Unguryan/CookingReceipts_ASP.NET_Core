namespace Interfaces.RabbitMQ
{
    public interface IRabbitBus
    {

        Task SendAsync<T>(string queue, T message) where T : class;

        Task ReceiveAsync<T>(string queue, Action<T> onMessage) where T : class;

    }
}
