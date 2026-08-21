namespace ICV.Application.DTOs.CvFile
{
    public class CvFileResponseDto
    {
        public int Id { get; set; }

        public int CvId { get; set; }

        public string OriginalFileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}