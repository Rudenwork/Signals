using MassTransit;
using MassTransit.Mediator;
using Signals.App.Core.Execution;
using Signals.App.Core.Notification;
using Signals.App.Database;
using Signals.App.Database.Entities.Channels;
using Signals.App.Database.Entities.Stages;
using Signals.App.Extensions;

namespace Signals.App.Core.Stage
{
    public class ExecuteNotification
    {
        public class Message
        {
            public Guid ExecutionId { get; set; }
            public NotificationStageEntity Stage { get; set; }
        }

        public class Consumer : IConsumer<Message>
        {
            private ILogger<Consumer> Logger { get; }
            private SignalsContext SignalsContext { get; }
            private IMediator Mediator { get; }

            public Consumer(ILogger<Consumer> logger, SignalsContext signalsContext, IMediator mediator)
            {
                Logger = logger;
                SignalsContext = signalsContext;
                Mediator = mediator;
            }

            public async Task Consume(ConsumeContext<Message> context)
            {
                context.EnsureFresh();

                Logger.LogInformation($"[{context.Message.ExecutionId}] Notification Stage {context.Message.Stage.Id}");

                var stage = context.Message.Stage;

                var signal = SignalsContext.Signals.Find(stage.SignalId);
                var channel = SignalsContext.Channels.Find(stage.ChannelId);

                var topic = $"{signal.Name} - {stage.Name}";
                var text = stage.Text;

                object request = channel switch
                {
                    EmailChannelEntity emailChannel => new SendEmailNotification.Request
                    { 
                        Channel = emailChannel,
                        Topic = topic,
                        Text = text
                    },
                    TelegramChannelEntity telegramChannel => new SendTelegramNotification.Request
                    { 
                        Channel = telegramChannel,
                        Topic = topic,
                        Text = text 
                    }
                };

                await Mediator.Send(request);

                await context.Publish(new Next.Message { ExecutionId = context.Message.ExecutionId });
            }
        }

        public class FaultConsumer : IConsumer<Fault<Message>>
        {
            public async Task Consume(ConsumeContext<Fault<Message>> context)
            {
                await context.Publish(new Stop.Message { ExecutionId = context.Message.Message.ExecutionId });
            }
        }
    }
}
