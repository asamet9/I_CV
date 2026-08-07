using ICV.Application.Interfaces.Repositories;
using ICV.Domain.Entities;
using ICV.Infrastructure.Persistence.Context;

namespace ICV.Infrastructure.Persistence.Repositories
{
    public class CvSectionRepository : GenericRepository<CvSection>, ICvSectionRepository
    {
        public CvSectionRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}