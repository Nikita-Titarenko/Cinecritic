using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Cinecritic.Application.DTOs.Account
{
    public class ChangeDisplayNameDto
    {
        public ObjectId UserId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }
}
