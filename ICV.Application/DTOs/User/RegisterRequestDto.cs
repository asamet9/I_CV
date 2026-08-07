using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICV.Application.DTOs.User
{
    public class RegisterRequestDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string PreferredLanguage { get; set; } = "en";


    }
}
