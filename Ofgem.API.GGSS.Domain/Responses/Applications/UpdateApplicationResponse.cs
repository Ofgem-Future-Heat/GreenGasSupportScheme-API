using System.Collections.Generic;

namespace Ofgem.API.GGSS.Domain.Responses.Applications
{
    public class UpdateApplicationResponse
    {
        public List<string> Errors { get; set; } = new List<string>();
    }
}