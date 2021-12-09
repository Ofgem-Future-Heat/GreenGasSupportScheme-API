using System;

namespace Ofgem.API.GGSS.Application.Entities
{
    public interface IDbEntity
    {
        Guid Id { get; set; }
    }
}
