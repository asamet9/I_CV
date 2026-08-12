using ICV.Domain.Common;
using ICV.Domain.Entities;

namespace ICV.Domain.Entities
{
    public class CvAnalysis : BaseEntity
    {
      
        public int CvId { get; set; }  // Analiz edilen CV'nin ID'si.

    
        public int ProfessionId { get; set; }    // Analizin hangi mesleğe göre yapıldığı.


        public int MatchedSkillCount { get; set; }        // CV'de bulunan meslek kriterlerinin sayısı.

    
        public int MissingSkillCount { get; set; }    // CV'de bulunamayan meslek kriterlerinin sayısı.


        public decimal Score { get; set; }        // CV'nin genel uyumluluk skoru. // Örneğin: 75.50

       
       
        public Cv Cv { get; set; } = null!; // Navigation Property.

       
        public Profession Profession { get; set; } = null!; // Navigation Property.
    }
}