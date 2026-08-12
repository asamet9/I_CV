using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICV.Application.DTOs.CvAnalysis;

namespace ICV.Application.Interfaces.Services
{
    /// <summary>
    /// CV analiz işlemlerini yöneten servis sözleşmesidir.
    ///
    /// Burada analiz işleminin NASIL yapılacağını değil,
    /// servisin HANGİ işlemleri sunacağını tanımlarız.
    ///
    /// Asıl implementasyon daha sonra
    /// CvAnalysisService sınıfında yapılacaktır.
    /// </summary>
    public interface ICvAnalysisService
    {
        /// <summary>
        /// Kullanıcının CV'sini seçilen mesleğe göre analiz eder.
        ///
        /// Bu işlem sırasında ileride:
        /// 1. CV kontrol edilecek.
        /// 2. Meslek kontrol edilecek.
        /// 3. Mesleğin kriterleri bulunacak.
        /// 4. CV içeriği incelenecek.
        /// 5. Skill eşleşmeleri yapılacak.
        /// 6. Eksik skill'ler belirlenecek.
        /// 7. CV skoru hesaplanacak.
        /// 8. CvAnalysis kaydı oluşturulacak.
        /// 9. Gerekirse SkillSuggestion kayıtları oluşturulacak.
        /// </summary>
        /// 
        /// <param name="cvId">
        /// Analiz edilecek CV'nin ID'si.
        /// </param>
        ///
        /// <param name="request">
        /// Analizin hangi meslek için yapılacağını
        /// belirleyen request DTO'su.
        /// </param>
        ///
        /// <param name="userId">
        /// JWT üzerinden gelen giriş yapmış kullanıcının ID'si.
        /// Böylece kullanıcı sadece kendi CV'sini analiz edebilir.
        /// </param>
        ///
        /// <returns>
        /// Analiz tamamlandığında oluşan analiz sonucunu döndürür.
        /// </returns>
        Task<CvAnalysisResponseDto> AnalyzeAsync(
            int cvId,
            AnalyzeCvRequestDto request,
            int userId);
    }
}