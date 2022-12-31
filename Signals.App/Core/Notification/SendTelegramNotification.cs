using MassTransit;
using Signals.App.Database.Entities.Channels;

namespace Signals.App.Core.Notification
{
    public class SendTelegramNotification
    {
        public class Request
        {
            public TelegramChannelEntity Channel { get; set; }
            public string Topic { get; set; }
            public string Message { get; set; }
        }

        public class Consumer : IConsumer<Request>
        {
            public async Task Consume(ConsumeContext<Request> context)
            {
                
            }
        }
    }
}
