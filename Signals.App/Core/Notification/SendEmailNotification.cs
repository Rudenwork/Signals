using MailKit.Net.Smtp;
using MassTransit;
using Microsoft.Extensions.Options;
using MimeKit;
using Signals.App.Settings;

namespace Signals.App.Core.Notification
{
    public class SendEmailNotification
    {
        public class Request
        {
            public string Address { get; set; }
            public string Topic { get; set; }
            public string Text { get; set; }
        }

        public class Consumer : IConsumer<Request>
        {
            private AppSettings.EmailSettings Settings { get; }

            public Consumer(IOptions<AppSettings> options)
            {
                Settings = options.Value.Email;
            }

            public async Task Consume(ConsumeContext<Request> context)
            {
                var message = new MimeMessage 
                {
                    From = { new MailboxAddress("Signals", Settings.Username) },
                    To = { new MailboxAddress("User", context.Message.Address) },
                    Subject = context.Message.Topic,
                    Body = new TextPart("plain") { Text = context.Message.Text }
                };

                using var client = new SmtpClient();

                client.Connect(Settings.Host, Settings.Port, true);
                client.Authenticate(Settings.Username, Settings.Password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
