using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICV.Application.DTOs.Profession;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;

namespace ICV.Application.Services
{
    public class ProfessionService : IProfessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProfessionResponseDto> CreateAsync(
            CreateProfessionRequestDto request)
        {
            var profession = new Profession
            {
                Name = request.Name
            };

            await _unitOfWork.Professions.AddAsync(profession);

            await _unitOfWork.SaveChangesAsync();

            return new ProfessionResponseDto
            {
                Id = profession.Id,
                Name = profession.Name,
                CreatedAt = profession.CreatedAt
            };
        }

        public async Task<IEnumerable<ProfessionResponseDto>> GetAllAsync()
        {
            var professions = await _unitOfWork.Professions
                .GetAllAsync();

            return professions
                .Select(x => new ProfessionResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreatedAt = x.CreatedAt
                });
        }

        public async Task<ProfessionResponseDto?> GetByIdAsync(
            int professionId)
        {
            var profession = await _unitOfWork.Professions
                .GetByIdAsync(professionId);

            if (profession == null)
                return null;

            return new ProfessionResponseDto
            {
                Id = profession.Id,
                Name = profession.Name,
                CreatedAt = profession.CreatedAt
            };
        }

        public async Task<ProfessionResponseDto?> UpdateAsync(
            int professionId,
            UpdateProfessionRequestDto request)
        {
            var profession = await _unitOfWork.Professions
                .GetByIdAsync(professionId);

            if (profession == null)
                return null;

            profession.Name = request.Name;

            _unitOfWork.Professions.Update(profession);

            await _unitOfWork.SaveChangesAsync();

            return new ProfessionResponseDto
            {
                Id = profession.Id,
                Name = profession.Name,
                CreatedAt = profession.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(
            int professionId)
        {
            var profession = await _unitOfWork.Professions
                .GetByIdAsync(professionId);

            if (profession == null)
                return false;

            _unitOfWork.Professions.Delete(profession);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}