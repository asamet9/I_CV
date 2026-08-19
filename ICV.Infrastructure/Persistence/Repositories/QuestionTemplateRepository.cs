using ICV.Application.Interfaces.Repositories;
using ICV.Domain.Entities;
using ICV.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ICV.Infrastructure.Persistence.Repositories
{
    public class QuestionTemplateRepository
        : GenericRepository<QuestionTemplate>,
          IQuestionTemplateRepository
    {
        public QuestionTemplateRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<QuestionTemplate?> GetByIdWithOptionsAsync(int id)
        {
            return await _dbSet
                .Include(x => x.Options)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<QuestionTemplate>> GetAllWithOptionsAsync(
            int professionId)
        {
            return await _dbSet
                .Where(x => x.ProfessionId == professionId)
                .Include(x => x.Options.OrderBy(o => o.OrderIndex))
                .ToListAsync();
        }
    }
}