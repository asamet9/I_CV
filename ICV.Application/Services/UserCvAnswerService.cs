
using ICV.Application.DTOs.UserCvAnswer;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;

namespace ICV.Application.Services
{
    public class UserCvAnswerService : IUserCvAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserCvAnswerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserCvAnswerResponseDto> CreateAsync(
            int userId,
            CreateUserCvAnswerRequestDto request)
        {
            // CV gerçekten bu kullanıcıya mı ait?
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == request.CvId &&
                    x.UserId == userId);

            if (cv == null)
                throw new KeyNotFoundException(
                    "CV bulunamadı veya bu CV size ait değil.");

            // Soru gerçekten var mı?
            var question = await _unitOfWork.QuestionTemplates
                .GetByIdAsync(request.QuestionTemplateId);

            if (question == null)
                throw new KeyNotFoundException(
                    "Belirtilen soru bulunamadı.");

            // Soru bu CV'nin mesleğine ait mi?
            if (question.ProfessionId != cv.ProfessionId)
                throw new InvalidOperationException(
                    "Bu soru seçilen CV'ye ait değil.");

            // Aynı CV'de aynı soru daha önce cevaplanmış mı?
            var existingAnswer = await _unitOfWork.UserCvAnswers
                .FirstOrDefaultAsync(x =>
                    x.CvId == request.CvId &&
                    x.QuestionTemplateId == request.QuestionTemplateId);

            if (existingAnswer != null)
            {
                existingAnswer.Answer = request.Answer;

                _unitOfWork.UserCvAnswers.Update(existingAnswer);

                await _unitOfWork.SaveChangesAsync();

                return MapToDto(existingAnswer);
            }

            // Yeni cevap oluştur.
            var answer = new UserCvAnswer
            {
                CvId = request.CvId,
                QuestionTemplateId = request.QuestionTemplateId,
                Answer = request.Answer
            };

            await _unitOfWork.UserCvAnswers
                .AddAsync(answer);

            await _unitOfWork.SaveChangesAsync();

            return MapToDto(answer);
        }

        public async Task<IEnumerable<UserCvAnswerResponseDto>> GetByCvIdAsync(
            int userId,
            int cvId)
        {
            // CV kullanıcıya ait mi?
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == cvId &&
                    x.UserId == userId);

            if (cv == null)
                throw new KeyNotFoundException(
                    "CV bulunamadı veya bu CV size ait değil.");

            var answers = await _unitOfWork.UserCvAnswers
                .FindAsync(x => x.CvId == cvId);

            return answers.Select(MapToDto);
        }

        public async Task<UserCvAnswerResponseDto?> GetByIdAsync(
            int userId,
            int answerId)
        {
            var answer = await _unitOfWork.UserCvAnswers
                .GetByIdAsync(answerId);

            if (answer == null)
                return null;

            // Cevabın bağlı olduğu CV kullanıcıya ait mi?
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == answer.CvId &&
                    x.UserId == userId);

            if (cv == null)
                return null;

            return MapToDto(answer);
        }

        public async Task<bool> DeleteAsync(
            int userId,
            int answerId)
        {
            var answer = await _unitOfWork.UserCvAnswers
                .GetByIdAsync(answerId);

            if (answer == null)
                return false;

            // Cevabın bağlı olduğu CV kullanıcıya ait mi?
            var cv = await _unitOfWork.Cvs
                .FirstOrDefaultAsync(x =>
                    x.Id == answer.CvId &&
                    x.UserId == userId);

            if (cv == null)
                return false;

            _unitOfWork.UserCvAnswers.Delete(answer);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static UserCvAnswerResponseDto MapToDto(
            UserCvAnswer answer)
        {
            return new UserCvAnswerResponseDto
            {
                Id = answer.Id,
                CvId = answer.CvId,
                QuestionTemplateId = answer.QuestionTemplateId,
                Answer = answer.Answer,
                CreatedAt = answer.CreatedAt
            };
        }
    }
}

