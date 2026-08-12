using ICV.Application.Interfaces.Repositories;
using ICV.Domain.Entities;
using ICV.Infrastructure.Persistence.Context;

namespace ICV.Infrastructure.Persistence.Repositories
{
    public class CvAnalysisRepository : GenericRepository<CvAnalysis>, ICvAnalysisRepository
    {
        public CvAnalysisRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}