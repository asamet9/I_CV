using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Domain.Common;  // miras almak için eklemelisin 

namespace ICV.Domain.Entities
{
    public class User : BaseEntity //miras aldı baseentitiden 
    {
        public string Email { get; set; } = string.Empty; // boş geçilmesin diye yazılır sondaki 
        public string? PasswordHash { get; set; } 
        public string FullName {  get; set; } = string.Empty;
        public string PreferredLanguage { get; set; } = "en"; // varsayılan ingilizce olacak 

        // Bu kullanıcıya ait tüm CV'ler.
        public ICollection<Cv> Cvs { get; set; } = new List<Cv>();

    }
}
