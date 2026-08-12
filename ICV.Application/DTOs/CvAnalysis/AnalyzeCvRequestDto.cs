namespace ICV.Application.DTOs.CvAnalysis
{
    /// <summary>
    /// Bir CV'nin belirli bir mesleğe göre analiz edilmesi
    /// için API'ye gönderilen verileri temsil eder.
    /// </summary>
    public class AnalyzeCvRequestDto
    {
        // ---------------------------------------------------------
        // ANALİZİN YAPILACAĞI MESLEK
        // ---------------------------------------------------------

        // CV hangi meslek açısından değerlendirilecek?
        //
        // Örneğin:
        // 1 = Computer Engineering
        // 2 = Mechanical Engineering
        //
        // Bu ID sayesinde sistem hangi mesleğin kriterlerini
        // kullanacağını bilecek.
        public int ProfessionId { get; set; }
    }
}