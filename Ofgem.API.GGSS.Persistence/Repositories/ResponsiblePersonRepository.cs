using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Persistence.Contracts;

namespace Ofgem.API.GGSS.Persistence.Repositories
{
    public class ResponsiblePersonRepository : BaseRepository<ResponsiblePerson>, IResponsiblePersonRepository
    {
        public ResponsiblePersonRepository(IDbContextFactory factory) : base(factory)
        {
        }
    }
}