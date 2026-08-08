using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.DTOs.User
{
    public class UpdateUserRequestDto
    {
        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string PreferredLanguage { get; set; } = "en";
    }
}