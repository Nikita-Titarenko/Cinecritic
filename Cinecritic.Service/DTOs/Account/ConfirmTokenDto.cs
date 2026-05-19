namespace Cinecritic.Application.DTOs.Account
{
    public class ConfirmTokenDto
    {
        public int UserId { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}