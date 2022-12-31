using Microsoft.Extensions.Options;
using Signals.App.Database;
using Signals.App.Database.Entities.Channels;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Signals.App.Extensions
{
    public static class TelegramExtensions
    {
        public static IServiceCollection AddTelegramBot(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUpdateHandler, TelegramUpdateHandler>();

            var provider = serviceCollection.BuildServiceProvider();

            var settings = provider.GetService<IOptions<Settings.Settings>>().Value.Telegram;
            var updateHandler = provider.GetService<IUpdateHandler>();

            var client = new TelegramBotClient(settings.Token);
            client.StartReceiving(updateHandler);

            return serviceCollection;
        }
    }

    public class TelegramUpdateHandler : IUpdateHandler
    {
        private SignalsContext SignalsContext { get; }

        public TelegramUpdateHandler(SignalsContext signalsContext)
        {
            SignalsContext = signalsContext;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is not { } text)
                return;

            var chatId = message.Chat.Id;

            var channel = SignalsContext.Channels.FirstOrDefault(x => (x as TelegramChannelEntity).Username == message.From.Username) as TelegramChannelEntity;

            if (channel is null)
            {
                await client.SendTextMessageAsync(chatId, $"Channel is not created for your username");
                return;
            }

            if (channel.ChatId is null)
            {
                channel.ChatId = chatId;
                SignalsContext.Update(channel);
                SignalsContext.SaveChanges();
            }

            if (channel.IsVerified)
                return;

            await client.SendTextMessageAsync(chatId, $"Verification Code:\n{channel.Code}");
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
