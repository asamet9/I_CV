using ICV.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Entities;

namespace ICV.Application.Interfaces.Repositories
{
    public interface IQuestionTemplateRepository
        : IGenericRepository<QuestionTemplate>
    {
        Task<QuestionTemplate?> GetByIdWithOptionsAsync(int id);

        Task<IEnumerable<QuestionTemplate>> GetAllWithOptionsAsync(
            int professionId);
    }
}