using MediatR;
using UploadService.Application.Commands;
using UploadService.Domain.Entities;
using UploadService.Domain.Enums;
using UploadService.Domain.Interfaces;

namespace UploadService.Application.Handlers;

public class UploadReceiptCommandHandler : IRequestHandler<UploadReceiptCommand, Guid>
{
    private readonly IReceiptRepository _repository;

    public UploadReceiptCommandHandler(IReceiptRepository repository)
    {
        _repository = repository;
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

        // TODO: Raise OCR event (RabbitMQ veya EventBus ile)

        return file.Id;
    }
}
