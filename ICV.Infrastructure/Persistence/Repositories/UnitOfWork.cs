using ICV.Application.Interfaces.Repositories;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;
using ICV.Infrastructure.Persistence.Context;

namespace ICV.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IUserRepository Users { get; }

        public ICvRepository Cvs { get; }

        public IProfessionRepository Professions { get; }

        public ICvSectionRepository CvSections { get; }

        public ICvSectionItemRepository CvSectionItems { get; }

        public ISkillSuggestionRepository SkillSuggestions { get; }

        public ICourseRecommendationRepository CourseRecommendations { get; }

        public IQuestionTemplateRepository QuestionTemplates { get; }

        public IUserSkillProgressRepository UserSkillProgresses { get; }

        public ICvFileRepository CvFiles { get; }

        public IGenericRepository<UserCvAnswer> UserCvAnswers { get; }


        public ICvAnalysisRepository CvAnalyses { get; }

        public IGenericRepository<SkillDevelopmentGoal>
            SkillDevelopmentGoals
        { get; }

        public ICourseRepository Courses { get; }
        public IGenericRepository<QuestionOption> QuestionOptions { get; }
        public UnitOfWork(
            ApplicationDbContext context,
            IUserRepository userRepository,
            ICvRepository cvRepository,
            IProfessionRepository professionRepository,
            ICvSectionRepository cvSectionRepository,
            ICvSectionItemRepository cvSectionItemRepository,
            ISkillSuggestionRepository skillSuggestionRepository,
            ICourseRecommendationRepository courseRecommendationRepository,
            IQuestionTemplateRepository questionTemplateRepository,
            IUserSkillProgressRepository userSkillProgressRepository,
            ICvAnalysisRepository cvAnalysisRepository,
            ICvFileRepository cvFileRepository,
            ICourseRepository courseRepository,
            IGenericRepository<SkillDevelopmentGoal>
                skillDevelopmentGoalRepository)
        {
            CvFiles = cvFileRepository;

            _context = context;

            Users =
                userRepository;

            Cvs =
                cvRepository;

            Professions =
                professionRepository;

            CvSections =
                cvSectionRepository;

            CvSectionItems =
                cvSectionItemRepository;

            SkillSuggestions =
                skillSuggestionRepository;

            QuestionOptions = new GenericRepository<QuestionOption>(_context);
          
UserCvAnswers = new GenericRepository<UserCvAnswer>(_context);



            CourseRecommendations =
                courseRecommendationRepository;

            QuestionTemplates =
                questionTemplateRepository;

            UserSkillProgresses =
                userSkillProgressRepository;

            CvAnalyses =
                cvAnalysisRepository;

            Courses =
                courseRepository;

            SkillDevelopmentGoals =
                skillDevelopmentGoalRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}