namespace Signals.App.Settings
{
    public class AppSettings
    {
        public AdministrationSettings Administration { get; set; }
        public IdentitySettings Identity { get; set; }
        public RabbitMqSettings RabbitMq { get; set; }
        public EmailSettings Email { get; set; }
        public TelegramSettings Telegram { get; set; }
        public BinanceSettings Binance { get; set; }

        public class AdministrationSettings
        {
            public string DefaultUsername { get; set; }
            public string DefaultPassword { get; set; }
        }

        public class IdentitySettings
        {
            public string Authority { get; set; }
        }

        public class RabbitMqSettings
        {
            public string Host { get; set; }
            public ushort Port { get; set; }
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

        public class BinanceSettings
        {
            public string BaseAddress { get; set; }
        }
    }
}
