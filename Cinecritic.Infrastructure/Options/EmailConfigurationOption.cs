﻿namespace Cinecritic.Web.Options
{
    public class EmailConfigurationOption
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Server { get; set; } = string.Empty;

        public int Port { get; set; }
    }
}
