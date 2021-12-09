using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Domain.Contracts.Services
{
    public interface IApplicationService
    {
        Task<string> SaveStageOneAsync(StageOne stageOne, CancellationToken cancellationToken = default);
        Task<ApplicationModel> GetAsync(string aplicationId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ApplicationModel>> GetAsync(CancellationToken token = default, bool includeDocuments = true);
    }
}
