using MailKit.Net.Smtp;
using MassTransit;
using Microsoft.Extensions.Options;
using MimeKit;
using Signals.App.Database.Entities.Channels;
using static Signals.App.Settings.Settings;

namespace Signals.App.Core.Notification
{
    public class SendEmailNotification
    {
        public class Request
        {
            public EmailChannelEntity Channel { get; set; }
            public string Topic { get; set; }
            public string Message { get; set; }
        }

        public class Consumer : IConsumer<Request>
        {
            private EmailSettings Settings { get; }

            public Consumer(IOptions<Settings.Settings> options)
            {
                Settings = options.Value.Email;
            }

            public async Task Consume(ConsumeContext<Request> context)
            {
                var message = new MimeMessage 
                {
                    From = { new MailboxAddress("Signals", Settings.Username) },
                    To = { new MailboxAddress("User", context.Message.Channel.Address) },
                    Subject = context.Message.Topic,
                    Body = new TextPart("plain") { Text = context.Message.Message }
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
