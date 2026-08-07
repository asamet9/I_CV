using ICV.Application.DTOs.User;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;

namespace ICV.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            // Aynı email ile daha önce kayıt olunmuş mu?
            bool emailExists = await _unitOfWork.Users.AnyAsync(x => x.Email == request.Email);

            if (emailExists)
            {
                throw new Exception("Bu e-posta adresi zaten kayıtlı.");
            }

            // Yeni kullanıcı oluştur
            var user = new User
            {
                Email = request.Email,
                FullName = request.FullName,

                // Şimdilik düz kaydediyoruz.
                // Bir sonraki derste BCrypt ile hashleyeceğiz.
                PasswordHash = request.Password,

                PreferredLanguage = string.IsNullOrWhiteSpace(request.PreferredLanguage)
                    ? "en"
                    : request.PreferredLanguage
            };

            // Veritabanına ekle
            await _unitOfWork.Users.AddAsync(user);

            // Kaydet
            await _unitOfWork.SaveChangesAsync();

            // Client'a dönecek DTO
            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PreferredLanguage = user.PreferredLanguage
            };
        }
    }
}