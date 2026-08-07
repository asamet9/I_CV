using ICV.Application.Interfaces.Repositories;
using ICV.Domain.Entities;
using ICV.Infrastructure.Persistence.Context;

namespace ICV.Infrastructure.Persistence.Repositories
{
    public class SkillSuggestionRepository : GenericRepository<SkillSuggestion>, ISkillSuggestionRepository
    {
        public SkillSuggestionRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}