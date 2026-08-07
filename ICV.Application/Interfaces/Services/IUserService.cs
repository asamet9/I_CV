using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Application.DTOs;
using ICV.Application.DTOs.User;
using ICV.Domain.Entities;

namespace ICV.Application.Interfaces.Services
{
    public interface IUserService
    {

        Task<UserResponseDto> RegisterAsync(RegisterRequestDto request);

    }
}
