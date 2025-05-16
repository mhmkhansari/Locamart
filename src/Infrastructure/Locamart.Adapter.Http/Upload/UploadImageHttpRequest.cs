using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Locamart.Adapter.Http.Upload;

public class UploadImageHttpRequest
{
    [FromForm]
    public IFormFile File { get; set; } = default!;
    public string FileName { get; set; }
    public string ContentType { get; set; }
}

