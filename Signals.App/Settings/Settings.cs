namespace Signals.App.Settings
{
    public class Settings
    {
        public IdentitySettings Identity { get; set; }

        public RabbitMqSettings RabbitMq { get; set; }

        public EmailSettings Email { get; set; }

        public TelegramSettings Telegram { get; set; }

        public class IdentitySettings
        {
            public string Authority { get; set; }
        }

        public class RabbitMqSettings
        {
            public string Host { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class EmailSettings
        {
            public string Host { get; set; }
            public int Port { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class TelegramSettings
        {
            public string Token { get; set; }
        }
    }
}
