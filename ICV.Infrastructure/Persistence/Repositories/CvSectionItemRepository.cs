using ICV.Application.Interfaces.Repositories;
using ICV.Domain.Entities;
using ICV.Infrastructure.Persistence.Context;

namespace ICV.Infrastructure.Persistence.Repositories
{
    public class CvSectionItemRepository : GenericRepository<CvSectionItem>, ICvSectionItemRepository
    {
        public CvSectionItemRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}