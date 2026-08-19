
namespace ICV.Application.DTOs.UserCvAnswer
{
    public class CreateUserCvAnswerRequestDto
    {
        public int CvId { get; set; }

        public int QuestionTemplateId { get; set; }

        public string Answer { get; set; } = string.Empty;
    }
}
