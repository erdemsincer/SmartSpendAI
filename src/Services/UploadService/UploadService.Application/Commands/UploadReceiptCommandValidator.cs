using FluentValidation;

namespace UploadService.Application.Commands;

public class UploadReceiptCommandValidator : AbstractValidator<UploadReceiptCommand>
{
    public UploadReceiptCommandValidator()
    {
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.FilePath).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FileType).IsInEnum();
    }
}
