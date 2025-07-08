using MediatR;
using UploadService.Application.Commands;
using UploadService.Application.Common.Messaging;
using UploadService.Domain.Entities;
using UploadService.Domain.Enums;
using UploadService.Domain.Interfaces;
using OCRService.Shared.Events;

namespace UploadService.Application.Handlers;

public class UploadReceiptCommandHandler : IRequestHandler<UploadReceiptCommand, Guid>
{
    private readonly IReceiptRepository _repository;
    private readonly IEventPublisher _eventPublisher;

    public UploadReceiptCommandHandler(IReceiptRepository repository, IEventPublisher eventPublisher)
    {
        _repository = repository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Guid> Handle(UploadReceiptCommand request, CancellationToken cancellationToken)
    {
        var file = new ReceiptFile
        {
            UserId = request.UserId,
            FileName = request.FileName,
            FilePath = request.FilePath,
            FileType = (FileType)request.FileType,
            UploadedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(file);

        var @event = new FileUploadedEvent
        {
            FileId = file.Id,
            UserId = file.UserId,
            FilePath = file.FilePath
        };

        await _eventPublisher.PublishAsync(@event, "file_uploaded");

        return file.Id;
    }
}
