using ICV.Application.Interfaces.Repositories;
using ICV.Domain.Entities;
using ICV.Infrastructure.Persistence.Context;

namespace ICV.Infrastructure.Persistence.Repositories
{
    public class UserSkillProgressRepository : GenericRepository<UserSkillProgress>, IUserSkillProgressRepository
    {
        public UserSkillProgressRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}