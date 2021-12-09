using Microsoft.AspNetCore.Http;

namespace Ofgem.API.GGSS.Application.Models.Upload
{
    public class FileModel
    {
       public IFormFile File { get; set; }
    }
}
