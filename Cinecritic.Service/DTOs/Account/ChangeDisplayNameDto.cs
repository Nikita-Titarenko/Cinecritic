using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinecritic.Application.DTOs.Account
{
    public class ChangeDisplayNameDto
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }
}
