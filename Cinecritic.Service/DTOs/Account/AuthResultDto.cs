namespace Cinecritic.Application.DTOs.Account
{
    public class AuthResultDto
    {
        public bool EmailNotConfirmed { get; set; }
        public bool PasswordNotExist { get; set; }
        public int? UserId { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
