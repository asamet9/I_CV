using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;
using ICV.Domain.Common;


namespace ICV.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {

        // <t> generic demek. yani t yerine istediğimiz entity gelecek. // "T sadece BaseEntity'den türeyen sınıflar olabilir." 



        // Id'ye göre tek kayıt getirir.
        Task<T?> GetByIdAsync(int id);

        // Tablodaki tüm kayıtları getirir. // Ienumerable liste demek birden fazal tutar
        Task<IEnumerable<T>> GetAllAsync();

        // Yeni kayıt ekler.
        Task AddAsync(T entity);

        // Kayıt günceller.
        void Update(T entity);

        // Kayıt siler.
        void Delete(T entity);

        // Şarta göre kayıt getirir.
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // İlk eşleşen kaydı getirir.
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        // Kayıt var mı kontrol eder.
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    }
}
