using ICV.Application.Interfaces.Repositories;
using ICV.Domain.Entities;
using ICV.Infrastructure.Persistence.Context;
using ICV.Infrastructure.Persistence.Repositories;

public class CvRepository : GenericRepository<Cv>, ICvRepository
{
    public CvRepository(ApplicationDbContext context)
        : base(context)
    {
    }
}