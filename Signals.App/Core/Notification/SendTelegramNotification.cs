using MassTransit;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Signals.App.Settings;

namespace Signals.App.Core.Notification
{
    public class SendTelegramNotification
    {
        public class Request
        {
            public long ChatId { get; set; }
            public string Topic { get; set; }
            public string Text { get; set; }
        }

        public class Consumer : IConsumer<Request>
        {
            private AppSettings.TelegramSettings Settings { get; }

            public Consumer(IOptions<AppSettings> options)
            {
                Settings = options.Value.Telegram;
            }

            public async Task Consume(ConsumeContext<Request> context)
            {
                var message = context.Message;

                var client = new TelegramBotClient(Settings.Token);

                await client.SendTextMessageAsync(message.ChatId, $"[{message.Topic}]\n{message.Text}");
            }
        }
    }
}
