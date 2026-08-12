using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Application.Interfaces.Repositories;


namespace ICV.Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }

        ICvRepository Cvs { get; }

        IProfessionRepository Professions { get; }

        ICvSectionRepository CvSections { get; }

        ICvSectionItemRepository CvSectionItems { get; }

        ISkillSuggestionRepository SkillSuggestions { get; }

        ICourseRecommendationRepository CourseRecommendations { get; }

        IQuestionTemplateRepository QuestionTemplates { get; }

        IUserSkillProgressRepository UserSkillProgresses { get; }

        ICvAnalysisRepository CvAnalyses { get; } // CV analiz sonuçlarına erişim sağlar.

        Task<int> SaveChangesAsync();
        void Dispose();


    }
}
