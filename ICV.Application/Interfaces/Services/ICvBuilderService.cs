using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using System.Threading.Tasks;

namespace ICV.Application.Interfaces.Services
{
    public interface ICvBuilderService
    {
        /// <summary>
        /// Kullanıcının CV sorularına verdiği cevapları
        /// gerçek CV yapısına dönüştürür.
        ///
        /// UserCvAnswer kayıtlarını okuyarak:
        /// - Gerekli CvSection'ları oluşturur.
        /// - Cevapları CvSectionItem kayıtlarına dönüştürür.
        /// - Mevcut CV verilerini günceller.
        /// </summary>
        Task BuildFromAnswersAsync(
            int cvId,
            int userId);
    }
}

