namespace UploadService.Application.Common.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event, string queueName);
}
