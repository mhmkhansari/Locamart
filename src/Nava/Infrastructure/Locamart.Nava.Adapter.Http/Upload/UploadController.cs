using Locamart.Adapter.Http.Upload;
using Locamart.Nava.Application.UseCases.Upload.AddImage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Locamart.Nava.Adapter.Http.Upload
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] UploadImageHttpRequest request, CancellationToken cancellationToken)
        {
            await using var uploadingFile = request.File.OpenReadStream();

            var uploadCommand = new AddImageCommand
            {
                File = uploadingFile,
                FileName = request.FileName,
                ContentType = request.ContentType
            };

            var result = await mediator.Send(uploadCommand, cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);

        }
    }
}
