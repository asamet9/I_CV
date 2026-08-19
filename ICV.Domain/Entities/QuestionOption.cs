using ICV.Domain.Common;

namespace ICV.Domain.Entities
{
    public class QuestionOption : BaseEntity
    {
        public int QuestionTemplateId { get; set; }

        public string OptionText { get; set; } = string.Empty;

        public string? OptionValue { get; set; }

        public int OrderIndex { get; set; }

        public QuestionTemplate QuestionTemplate { get; set; } = null!;
    }
}