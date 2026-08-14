using ICV.Application.DTOs.CourseRecommendation;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;

namespace ICV.Application.Services
{
    public class CourseRecommendationService : ICourseRecommendationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseRecommendationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Yeni kurs önerisi oluşturur.
        public async Task<CourseRecommendationResponseDto> CreateAsync(
            CreateCourseRecommendationRequestDto request,
            int userId)
        {
            // SkillSuggestion'ın gerçekten giriş yapan kullanıcıya
            // ait bir CV üzerinden geldiğini kontrol ediyoruz.
            var skillSuggestion = await _unitOfWork.SkillSuggestions
                .FirstOrDefaultAsync(x =>
                    x.Id == request.SkillSuggestionId &&
                    x.Cv.UserId == userId);

            if (skillSuggestion == null)
                throw new UnauthorizedAccessException(
                    "Bu skill önerisine erişim yetkiniz yok.");

            // Önerilmek istenen kursun sistemde bulunup
            // bulunmadığını kontrol ediyoruz.
            var course = await _unitOfWork.Courses
                .FirstOrDefaultAsync(x =>
                    x.Id == request.CourseId &&
                    x.IsActive);

            if (course == null)
                throw new KeyNotFoundException(
                    "Önerilmek istenen kurs bulunamadı veya aktif değil.");

            // Aynı kurs aynı SkillSuggestion için daha önce
            // önerilmiş mi kontrol ediyoruz.
            var alreadyRecommended =
                await _unitOfWork.CourseRecommendations
                    .AnyAsync(x =>
                        x.SkillSuggestionId == request.SkillSuggestionId &&
                        x.CourseId == request.CourseId);

            if (alreadyRecommended)
                throw new InvalidOperationException(
                    "Bu kurs zaten bu skill için önerilmiş.");

            // CourseRecommendation artık kurs bilgilerini
            // tekrar tutmuyor.
            // Sadece SkillSuggestion ile Course arasındaki
            // ilişkiyi oluşturuyoruz.
            var courseRecommendation = new CourseRecommendation
            {
                SkillSuggestionId = request.SkillSuggestionId,
                CourseId = request.CourseId
            };

            await _unitOfWork.CourseRecommendations
                .AddAsync(courseRecommendation);

            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(
                courseRecommendation,
                course);
        }


        // Belirli bir SkillSuggestion'a ait kurs önerilerini getirir.
        public async Task<IEnumerable<CourseRecommendationResponseDto>> GetAllAsync(
            int skillSuggestionId,
            int userId)
        {
            // SkillSuggestion'ın kullanıcıya ait olduğunu kontrol ediyoruz.
            var skillSuggestion = await _unitOfWork.SkillSuggestions
                .FirstOrDefaultAsync(x =>
                    x.Id == skillSuggestionId &&
                    x.Cv.UserId == userId);

            if (skillSuggestion == null)
                throw new UnauthorizedAccessException(
                    "Bu skill önerisine erişim yetkiniz yok.");

            var recommendations =
                await _unitOfWork.CourseRecommendations
                    .FindAsync(x =>
                        x.SkillSuggestionId == skillSuggestionId);

            var result = new List<CourseRecommendationResponseDto>();

            foreach (var recommendation in recommendations)
            {
                // Recommendation'ın bağlı olduğu gerçek Course'u getiriyoruz.
                var course = await _unitOfWork.Courses
                    .FirstOrDefaultAsync(x =>
                        x.Id == recommendation.CourseId);

                if (course == null)
                    continue;

                result.Add(
                    MapToResponse(
                        recommendation,
                        course));
            }

            return result;
        }


        // Tek bir kurs önerisini getirir.
        public async Task<CourseRecommendationResponseDto?> GetByIdAsync(
            int courseRecommendationId,
            int userId)
        {
            var recommendation =
                await _unitOfWork.CourseRecommendations
                    .FirstOrDefaultAsync(x =>
                        x.Id == courseRecommendationId &&
                        x.SkillSuggestion.Cv.UserId == userId);

            if (recommendation == null)
                return null;

            var course = await _unitOfWork.Courses
                .FirstOrDefaultAsync(x =>
                    x.Id == recommendation.CourseId);

            if (course == null)
                return null;

            return MapToResponse(
                recommendation,
                course);
        }


        // Kurs önerisini günceller.
        public async Task<CourseRecommendationResponseDto?> UpdateAsync(
            int courseRecommendationId,
            UpdateCourseRecommendationRequestDto request,
            int userId)
        {
            var recommendation =
                await _unitOfWork.CourseRecommendations
                    .FirstOrDefaultAsync(x =>
                        x.Id == courseRecommendationId &&
                        x.SkillSuggestion.Cv.UserId == userId);

            if (recommendation == null)
                return null;

            // Yeni Course gerçekten var mı?
            var course = await _unitOfWork.Courses
                .FirstOrDefaultAsync(x =>
                    x.Id == request.CourseId &&
                    x.IsActive);

            if (course == null)
                throw new KeyNotFoundException(
                    "Seçilen kurs bulunamadı veya aktif değil.");

            // Aynı kurs zaten bu SkillSuggestion'a
            // önerilmiş mi?
            var alreadyRecommended =
                await _unitOfWork.CourseRecommendations
                    .AnyAsync(x =>
                        x.Id != courseRecommendationId &&
                        x.SkillSuggestionId == recommendation.SkillSuggestionId &&
                        x.CourseId == request.CourseId);

            if (alreadyRecommended)
                throw new InvalidOperationException(
                    "Bu kurs zaten bu skill için önerilmiş.");

            // Sadece ilişkiyi değiştiriyoruz.
            recommendation.CourseId = request.CourseId;

            _unitOfWork.CourseRecommendations
                .Update(recommendation);

            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(
                recommendation,
                course);
        }


        // Kurs önerisini siler.
        public async Task<bool> DeleteAsync(
            int courseRecommendationId,
            int userId)
        {
            var recommendation =
                await _unitOfWork.CourseRecommendations
                    .FirstOrDefaultAsync(x =>
                        x.Id == courseRecommendationId &&
                        x.SkillSuggestion.Cv.UserId == userId);

            if (recommendation == null)
                return false;

            // Burada Course'u silmiyoruz.
            // Sadece kullanıcının önerisini siliyoruz.
            _unitOfWork.CourseRecommendations
                .Delete(recommendation);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }


        // Course ve CourseRecommendation bilgilerini
        // Response DTO'ya dönüştüren ortak metot.
        private static CourseRecommendationResponseDto MapToResponse(
            CourseRecommendation recommendation,
            Course course)
        {
            return new CourseRecommendationResponseDto
            {
                Id = recommendation.Id,

                SkillSuggestionId =
                    recommendation.SkillSuggestionId,

                CourseId =
                    recommendation.CourseId,

                // Bu bilgiler Course tablosundan geliyor.
                Title = course.Title,

                Provider = course.Provider,

                Url = course.Url,

                // Şu an bu alanlar CourseRecommendation tarafında
                // tutulduğu için burada henüz recommendation'dan
                // alınamıyor.
                //
                // Bunları birazdan netleştireceğiz.
                Price = default,
                Level = default,
                DurationWeeks = 0,

                CreatedAt =
                    recommendation.CreatedAt
            };
        }
    }
}