using MassTransit;
using Microsoft.Extensions.Options;
using Signals.App.Database.Entities.Channels;
using Telegram.Bot;
using static Signals.App.Settings.Settings;

namespace Signals.App.Core.Notification
{
    public class SendTelegramNotification
    {
        public class Request
        {
            public TelegramChannelEntity Channel { get; set; }
            public string Topic { get; set; }
            public string Text { get; set; }
        }

        public class Consumer : IConsumer<Request>
        {
            private TelegramSettings Settings { get; }

            public Consumer(IOptions<Settings.Settings> options)
            {
                Settings = options.Value.Telegram;
            }

            public async Task Consume(ConsumeContext<Request> context)
            {
                var message = context.Message;

                var client = new TelegramBotClient(Settings.Token);

                await client.SendTextMessageAsync(message.Channel.ChatId, $"[{message.Topic}]\n{message.Text}");
            }
        }
    }
}
