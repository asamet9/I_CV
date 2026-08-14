namespace ICV.Application.DTOs.CourseRecommendation
{
    public class CreateCourseRecommendationRequestDto
    {
        // Hangi skill önerisi için bu kurs öneriliyor?
        public int SkillSuggestionId { get; set; }

        // Hangi kurs öneriliyor?
        public int CourseId { get; set; }
    }
}