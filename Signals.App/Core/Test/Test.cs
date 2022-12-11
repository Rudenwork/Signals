using MassTransit;

namespace Signals.App.Core.Test
{
    public class Test
    {
        public class Message
        {
            public string Text { get; set; }
        }

        public class Consumer : IConsumer<Message>
        {
            public async Task Consume(ConsumeContext<Message> context)
            {
                Console.WriteLine($"[{context.Message.Text}]");
                throw new Exception("TEST");
            }
        }

        public class FaultConsumer : IConsumer<Fault<Message>>
        {
            public async Task Consume(ConsumeContext<Fault<Message>> context)
            {
                Console.WriteLine($"[{nameof(Test)}.{nameof(FaultConsumer)}] => [{context.Message.Exceptions.FirstOrDefault()?.Message}]");
            }
        }
    }
}
