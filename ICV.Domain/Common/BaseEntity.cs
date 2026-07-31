using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Domain.Common
{
    public abstract class BaseEntity // abstract çünkü bundan yeni öğe olmayacak sadece miras alacağız.
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } 



    }
}
