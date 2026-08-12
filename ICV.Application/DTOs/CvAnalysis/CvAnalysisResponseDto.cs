namespace ICV.Application.DTOs.CvAnalysis
{
    /// <summary>
    /// CV analiz işlemi tamamlandıktan sonra
    /// API tarafından döndürülen analiz sonucunu temsil eder.
    /// </summary>
    public class CvAnalysisResponseDto
    {
        // ---------------------------------------------------------
        // ANALİZ KAYDI
        // ---------------------------------------------------------

        // Oluşturulan analiz kaydının ID'si.
        public int Id { get; set; }


        // ---------------------------------------------------------
        // CV BİLGİSİ
        // ---------------------------------------------------------

        // Analiz edilen CV'nin ID'si.
        public int CvId { get; set; }


        // ---------------------------------------------------------
        // MESLEK BİLGİSİ
        // ---------------------------------------------------------

        // Analizin hangi meslek için yapıldığını gösterir.
        public int ProfessionId { get; set; }

        // Kullanıcıya sadece ID göstermek yerine
        // mesleğin adını da döndürüyoruz.
        //
        // Örneğin:
        // "Computer Engineering"
        public string ProfessionName { get; set; } = string.Empty;


        // ---------------------------------------------------------
        // ANALİZ SONUÇLARI
        // ---------------------------------------------------------

        // CV'de bulunan ve beklenen kriterlerle eşleşen
        // skill sayısı.
        public int MatchedSkillCount { get; set; }


        // CV'de bulunmayan / eksik olan skill sayısı.
        public int MissingSkillCount { get; set; }


        // CV'nin genel analiz skoru.
        //
        // Örneğin:
        // 82.50
        public decimal Score { get; set; }


        // ---------------------------------------------------------
        // ANALİZ TARİHİ
        // ---------------------------------------------------------

        // Analizin ne zaman gerçekleştirildiğini gösterir.
        public DateTime CreatedAt { get; set; }
    }
}