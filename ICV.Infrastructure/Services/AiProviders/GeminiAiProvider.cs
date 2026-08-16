using Google.GenAI; // Gemini API client'ını kullanmamızı sağlar.
using Google.GenAI.Types; // Gemini'nin Schema ve GenerateContentConfig tiplerini kullanmamızı sağlar.
using ICV.Application.DTOs.AI; // AiSkillSuggestionDto sınıfına erişmemizi sağlar.
using ICV.Application.Interfaces.AI; // IAiProvider interface'ini kullanmamızı sağlar.
using ICV.Infrastructure.Configuration; // GeminiOptions sınıfına erişmemizi sağlar.
using Microsoft.Extensions.Options; // IOptions<T> ile configuration değerlerini alır.
using System.Text.Json; // JSON cevabını C# nesnelerine dönüştürür.
using SchemaType = Google.GenAI.Types.Type; // System.Type ile Google.GenAI.Types.Type arasındaki isim çakışmasını çözer.

namespace ICV.Infrastructure.Services.AiProviders
{
    /// <summary>
    /// Google Gemini API ile iletişim kuran AI provider sınıfıdır.
    /// IAiProvider interface'ini uyguladığı için Application katmanı
    /// Gemini'ye doğrudan bağımlı değildir.
    /// </summary>
    public class GeminiAiProvider : IAiProvider
    {
        private readonly GeminiOptions _options; // Gemini API ayarlarını tutar.
        private readonly Client _client; // Gemini API'ye istek gönderecek SDK client'ıdır.

        /// <summary>
        /// GeminiAiProvider sınıfının constructor'ıdır.
        /// Dependency Injection tarafından otomatik olarak oluşturulur.
        /// </summary>
        public GeminiAiProvider(IOptions<GeminiOptions> options)
        {
            _options = options.Value; // User Secrets/appsettings içerisindeki Gemini ayarlarını alır.

            if (string.IsNullOrWhiteSpace(_options.ApiKey)) // API key boş veya sadece boşluklardan oluşuyor mu kontrol eder.
            {
                throw new ArgumentException(
                    "Gemini API key cannot be empty.", // API key yoksa anlamlı bir hata mesajı verir.
                    nameof(options)); // Hatanın options parametresinden kaynaklandığını belirtir.
            }

            _client = new Client(
                apiKey: _options.ApiKey); // Gemini SDK client'ını API key ile oluşturur.
        }

        /// <summary>
        /// CV içeriğini ve mesleği Gemini'ye göndererek
        /// geliştirilmesi önerilen skill'leri döndürür.
        /// </summary>
        public async Task<IEnumerable<AiSkillSuggestionDto>> GenerateSkillSuggestionsAsync(
            string cvContent, // Kullanıcının CV'sinden elde edilen metinsel içerik.
            string professionName, // Kullanıcının seçtiği meslek.
            CancellationToken cancellationToken = default) // İşlemi gerektiğinde iptal etmek için kullanılır.
        {
            if (string.IsNullOrWhiteSpace(cvContent)) // CV içeriğinin boş olup olmadığını kontrol eder.
            {
                throw new ArgumentException(
                    "CV content cannot be empty.", // CV boşsa hata mesajı verir.
                    nameof(cvContent)); // Hatanın cvContent parametresinden kaynaklandığını belirtir.
            }

            if (string.IsNullOrWhiteSpace(professionName)) // Meslek bilgisinin boş olup olmadığını kontrol eder.
            {
                throw new ArgumentException(
                    "Profession name cannot be empty.", // Meslek boşsa hata mesajı verir.
                    nameof(professionName)); // Hatanın professionName parametresinden kaynaklandığını belirtir.
            }

            var skillSuggestionSchema = new Schema // Gemini'nin döndüreceği JSON yapısını tanımlar.
            {
                Type = SchemaType.Array, // Ana JSON yapımızın bir array/list olacağını belirtir.

                Items = new Schema // Array içerisindeki her elemanın yapısını tanımlar.
                {
                    Type = SchemaType.Object, // Array içerisindeki her elemanın JSON object olacağını belirtir.

                    Properties = new Dictionary<string, Schema> // JSON object içerisindeki alanları tanımlar.
        {
            {
                "skill",
                new Schema
                {
                    Type = SchemaType.String, // Skill değerinin string olacağını belirtir.
                    Title = "Skill" // Alanın açıklayıcı adını belirtir.
                }
            },

            {
                "category",
                new Schema
                {
                    Type = SchemaType.String, // Category değerinin string olacağını belirtir.
                    Title = "Category" // Alanın açıklayıcı adını belirtir.
                }
            },

            {
                "reason",
                new Schema
                {
                    Type = SchemaType.String, // Reason değerinin string olacağını belirtir.
                    Title = "Reason" // Alanın açıklayıcı adını belirtir.
                }
            }
        },

                    Required = new List<string> // Her skill objesinde bulunması zorunlu alanları belirtir.
        {
            "skill",
            "category",
            "reason"
        },

                    PropertyOrdering = new List<string> // JSON alanlarının sırasını belirtir.
        {
            "skill",
            "category",
            "reason"
        }
                }
            };

            var prompt = $"""
    You are an expert career advisor and CV analyzer.

    Analyze the following CV for the profession: {professionName}

    Identify the most valuable technical or professional skills
    that the candidate should develop.

    Rules:
    - Suggest only relevant skills.
    - Do not suggest skills the candidate already clearly demonstrates.
    - Focus on skills that improve employability.
    - Avoid duplicate skills.
    - Keep the number of suggestions between 3 and 8.
    - Category should be a short category name such as Backend, Frontend,
      DevOps, Database, Programming, Cloud, Testing or Security.
    - Reason should briefly explain why the skill is valuable for this candidate.

    CV:
    {cvContent}
    """; // Gemini'ye CV analizinin kurallarını ve analiz edilecek CV'yi gönderir.

            var response = await _client.Models.GenerateContentAsync(
      model: _options.Model, // User Secrets içerisindeki Gemini modelini kullanır.
      contents: prompt, // Hazırladığımız CV analiz prompt'unu Gemini'ye gönderir.

      config: new GenerateContentConfig
      {
          ResponseMimeType = "application/json", // Gemini'den cevabın JSON formatında gelmesini ister.

          ResponseSchema = skillSuggestionSchema, // Gemini'ye döndüreceği JSON'un yapısını bildirir.

          Temperature = 0.2 // Daha düşük değer kullanarak cevapların daha tutarlı olmasını sağlar.
      },

      cancellationToken: cancellationToken); // İstek iptal edilirse Gemini çağrısının da iptal edilmesini sağlar.

            var responseText = response.Text; // Gemini'nin oluşturduğu metinsel cevabı alır.

            if (string.IsNullOrWhiteSpace(responseText)) // Gemini boş cevap döndürmüş mü kontrol eder.
            {
                return Enumerable.Empty<AiSkillSuggestionDto>(); // Boş cevap varsa boş liste döndürür.
            }
            try
            {
                var suggestions = JsonSerializer.Deserialize<List<AiSkillSuggestionDto>>(
                    responseText,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }); // Gemini'den gelen JSON'u AiSkillSuggestionDto listesine dönüştürür.

                return suggestions ?? Enumerable.Empty<AiSkillSuggestionDto>(); // Deserialize null dönerse boş liste döndürür.
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    "Gemini returned an invalid JSON response.",
                    ex); // Gemini beklediğimiz JSON formatında cevap vermezse anlamlı bir hata oluşturur.
            }
        }
    }
}