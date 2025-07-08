using MediatR;
using UploadService.Domain.Enums;

namespace UploadService.Application.Commands;

public class UploadReceiptCommand : IRequest<Guid>  // Dönüş: File ID
{
    public Guid UserId { get; init; }
    public string FileName { get; init; } = null!;
    public string FilePath { get; init; } = null!;
    public FileType FileType { get; init; }
}
