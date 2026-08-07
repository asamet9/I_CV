using ICV.Application.Interfaces.Repositories;
using ICV.Domain.Entities;
using ICV.Infrastructure.Persistence.Context;

namespace ICV.Infrastructure.Persistence.Repositories
{
    public class CourseRecommendationRepository : GenericRepository<CourseRecommendation>, ICourseRecommendationRepository
    {
        public CourseRecommendationRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}