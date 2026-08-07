using ICV.Application.Interfaces.Repositories;
using ICV.Domain.Entities;
using ICV.Infrastructure.Persistence.Context;

namespace ICV.Infrastructure.Persistence.Repositories
{
    public class QuestionTemplateRepository : GenericRepository<QuestionTemplate>, IQuestionTemplateRepository
    {
        public QuestionTemplateRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}