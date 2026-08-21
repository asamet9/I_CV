using ICV.Application.Interfaces.Repositories;
using ICV.Domain.Entities;
using ICV.Infrastructure.Persistence.Context;

namespace ICV.Infrastructure.Persistence.Repositories
{
    public class CvFileRepository : GenericRepository<CvFile>, ICvFileRepository
    {
        public CvFileRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}