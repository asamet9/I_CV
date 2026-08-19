
namespace ICV.Application.DTOs.UserCvAnswer
{
    public class UserCvAnswerResponseDto
    {
        public int Id { get; set; }

        public int CvId { get; set; }

        public int QuestionTemplateId { get; set; }

        public string Answer { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}

