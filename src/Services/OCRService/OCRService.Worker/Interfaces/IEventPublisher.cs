namespace OCRService.Worker.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event, string queueName);
}
