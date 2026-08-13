using ICV.Application.DTOs.CvAnalysis;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;

namespace ICV.Application.Services
{
    /// <summary>
    /// CV analiz işlemlerini gerçekleştiren servistir.
    /// CRUD işlemlerinden farklı olarak projenin
    /// CV analiz iş mantığını burada yönetiyoruz.
    /// </summary>
    public class CvAnalysisService : ICvAnalysisService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISkillSuggestionService _skillSuggestionService; // Eksik skill'leri öneriye dönüştürmek için kullanıyoruz.

        public CvAnalysisService(
            IUnitOfWork unitOfWork,
            ISkillSuggestionService skillSuggestionService)
        {
            _unitOfWork = unitOfWork;
            _skillSuggestionService = skillSuggestionService;
        }

        /// <summary>
        /// CV'yi seçilen mesleğe göre analiz eder.
        /// </summary>
        public async Task<CvAnalysisResponseDto> AnalyzeAsync(
            int cvId,
            AnalyzeCvRequestDto request,
            int userId)
        {

            // CV'yi buluyor ve aynı anda CV'nin giriş yapan kullanıcıya ait
            // olup olmadığını kontrol ediyoruz.
            // Önce sadece CV ID'sine göre CV'yi buluyoruz.
            var cv = await _unitOfWork.Cvs
        .FirstOrDefaultAsync(x =>
            x.Id == cvId &&
            x.UserId == userId);

            // CV yoksa veya başka kullanıcıya aitse erişime izin vermiyoruz.
            if (cv == null)
            {
                throw new UnauthorizedAccessException(
                    "Bu CV'ye erişim yetkiniz yok.");
            }


            // Kullanıcının seçtiği mesleği veritabanından buluyoruz.
            var profession = await _unitOfWork.Professions
                .FirstOrDefaultAsync(x =>
                    x.Id == request.ProfessionId);

            // Meslek bulunamadıysa analiz yapılamaz.
            if (profession == null)
            {
                throw new KeyNotFoundException(
                    "Belirtilen meslek bulunamadı.");
            }


            // Seçilen mesleğe ait analiz kriterlerini getiriyoruz.
            var questionTemplates = await _unitOfWork.QuestionTemplates
                .FindAsync(x =>
                    x.ProfessionId == request.ProfessionId);

            // Mesleğe ait hiç kriter yoksa CV'yi değerlendiremeyiz.
            if (!questionTemplates.Any())
            {
                throw new InvalidOperationException(
                    "Bu meslek için henüz analiz kriteri tanımlanmamış.");
            }


            // CV'nin içerisindeki bölümleri getiriyoruz.
            var cvSections = await _unitOfWork.CvSections
                .FindAsync(x =>
                    x.CvId == cvId);

            // CV içerisinde hiç bölüm yoksa analiz edilecek veri yoktur.
            if (!cvSections.Any())
            {
                throw new InvalidOperationException(
                    "Bu CV içerisinde analiz edilecek bölüm bulunamadı.");
            }


            // Section'ların içerisindeki gerçek CV kayıtlarını getiriyoruz.
            var cvSectionItems = await _unitOfWork.CvSectionItems
                .FindAsync(x =>
                    cvSections
                        .Select(s => s.Id)
                        .Contains(x.CvSectionId));

            // Section var ama içerisinde hiç kayıt yoksa analiz yapamayız.
            if (!cvSectionItems.Any())
            {
                throw new InvalidOperationException(
                    "Bu CV içerisinde analiz edilecek kayıt bulunamadı.");
            }


            // CV'de bulunan ve meslek kriterleriyle eşleşen
            // kriterlerin isimlerini burada tutacağız.
            var matchedTemplates = new List<string>();

            // CV'de bulunamayan meslek kriterlerini burada tutacağız.
           var missingTemplates = new List<MissingSkillDto>();


            // Her meslek kriterini tek tek kontrol ediyoruz.
            foreach (var template in questionTemplates)
            {
                // ExpectedValue boşsa sistemin CV'de ne arayacağını
                // bilemeyeceğimiz için bu kriteri atlıyoruz.
                if (string.IsNullOrWhiteSpace(template.ExpectedValue))
                {
                    continue;
                }

                // Aranacak değeri standart hale getiriyoruz.
                // Örneğin " ASP.NET Core " → "asp.net core"
                var expectedValue = NormalizeText(
                    template.ExpectedValue);

                // Başlangıçta kriterin CV'de bulunmadığını varsayıyoruz.
                bool isMatched = false;


                // CV içerisindeki bütün kayıtları tek tek kontrol ediyoruz.
                foreach (var item in cvSectionItems)
                {
                    // CV item'ının başlığını standart hale getiriyoruz.
                    var title = NormalizeText(item.Title);

                    // CV item'ının açıklamasını standart hale getiriyoruz.
                    var description = NormalizeText(item.Description);

                    // Aradığımız değer başlıkta veya açıklamada
                    // bulunuyorsa kriter eşleşmiş demektir.
                    if (title.Contains(expectedValue) ||
                        description.Contains(expectedValue))
                    {
                        isMatched = true;
                        break; // Aynı kriteri tekrar aramaya gerek yok.
                    }
                }


                // Kriter CV'de bulunduysa matched listesine ekliyoruz.
                if (isMatched)
                {
                    matchedTemplates.Add(template.ExpectedValue);
                }
                // Bulunamadıysa missing listesine ekliyoruz.
                else
                {
                    missingTemplates.Add(new MissingSkillDto
                    {
                        Skill = template.ExpectedValue,
                        Category = template.Category
                    });
                }
            }


            // Şimdilik burada duruyoruz.
            // Bir sonraki aşamada bu iki listenin sayılarını kullanarak
            // MatchedSkillCount, MissingSkillCount ve Score hesaplayacağız.
            // Eşleşen kriterlerin toplam sayısını alıyoruz.
            var matchedSkillCount = matchedTemplates.Count;

            // Eksik kriterlerin toplam sayısını alıyoruz.
            var missingSkillCount = missingTemplates.Count;

            // Toplam değerlendirilen kriter sayısını hesaplıyoruz.
            var totalSkillCount =
                matchedSkillCount + missingSkillCount;


            // Hiçbir kriter değerlendirilemediyse sıfıra bölme
            // hatası yaşamamak için skoru 0 olarak belirliyoruz.
            decimal score = totalSkillCount == 0
                ? 0
                : (decimal)matchedSkillCount / totalSkillCount * 100;

            // ---------------------------------------------------------
            // 8. ANALİZ SONUCUNU OLUŞTUR
            // ---------------------------------------------------------

            // Hesapladığımız analiz sonuçlarını CvAnalysis entity'sine
            // aktarıyoruz.
            //
            // Bu nesne henüz veritabanına kaydedilmiş değil.
            // Sadece kaydedilecek veriyi hazırlıyoruz.
            var analysis = new CvAnalysis
            {
                CvId = cvId,                                      // Analiz edilen CV
                ProfessionId = request.ProfessionId,             // Analiz yapılan meslek
                MatchedSkillCount = matchedSkillCount,            // Eşleşen kriter sayısı
                MissingSkillCount = missingSkillCount,            // Eksik kriter sayısı
                Score = score                                     // Hesaplanan skor
            };

            // ---------------------------------------------------------
            // 9. EKSİK BECERİLER İÇİN ÖNERİ OLUŞTUR
            // ---------------------------------------------------------

            // CV analizinde bulunamayan her kriter için
            // kullanıcıya bir SkillSuggestion oluşturuyoruz.
            await _skillSuggestionService.GenerateFromAnalysisAsync(
            cvId,
            missingTemplates,
            userId);

            // Analiz sonucunu veritabanına eklenmek üzere
            // CvAnalysis repository'sine gönderiyoruz.
            await _unitOfWork.CvAnalyses.AddAsync(analysis);
    
            // Yapılan değişiklikleri SQL Server'a kaydediyoruz.
            await _unitOfWork.SaveChangesAsync();

            return new CvAnalysisResponseDto
            {
                Id = analysis.Id,
                CvId = analysis.CvId,
                ProfessionId = analysis.ProfessionId,

                ProfessionName = profession.Name, // Analizin hangi meslek için yapıldığını response'a ekliyoruz.

                MatchedSkillCount = analysis.MatchedSkillCount,
                MissingSkillCount = analysis.MissingSkillCount,
                Score = analysis.Score,

                CreatedAt = analysis.CreatedAt, // Analizin oluşturulma tarihini response'a ekliyoruz.

                MatchedSkills = matchedTemplates,
                MissingSkills = missingTemplates,
            };

        }


        // Metinleri karşılaştırmadan önce standart hale getiriyoruz.
        private static string NormalizeText(string? text)
        {
            // Null veya boş metin varsa boş string döndürüyoruz.
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            // Baştaki/sondaki boşlukları temizliyor ve
            // tüm karakterleri küçük harfe çeviriyoruz.
            return text.Trim().ToLowerInvariant();
        }
    }
}