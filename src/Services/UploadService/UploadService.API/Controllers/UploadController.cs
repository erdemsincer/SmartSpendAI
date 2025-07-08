using MediatR;
using Microsoft.AspNetCore.Mvc;
using UploadService.Application.Commands;

namespace UploadService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly IMediator _mediator;

    public UploadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromBody] UploadReceiptCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { receiptId = id });
    }
}
