using MediatR;
using Microsoft.EntityFrameworkCore;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Services;

namespace Signals.App.Commands.Block
{
    public class EvaluateBlock
    {
        public class Command : IRequest<bool>
        {
            public Guid BlockId { get; set; }
        }

        private class Handler : IRequestHandler<Command, bool>
        {
            private SignalsContext SignalsContext { get; }
            private CommandService CommandService { get; }

            public Handler(SignalsContext signalsContext, CommandService commandService)
            {
                SignalsContext = signalsContext;
                CommandService = commandService;
            }

            public async Task<bool> Handle(Command command, CancellationToken cancellationToken)
            {
                var block = SignalsContext.Blocks
                    .Where(x => x.Id == command.BlockId)
                    .Include(x => x.Parameters)
                    .FirstOrDefault();

                var parameters = block.Parameters.ToDictionary(x => x.Key, x => x.Value);

                switch (block.Type)
                {
                    case BlockEntity.BlockType.Group:
                        return await CommandService.Execute(new EvaluateGroupBlock.Command
                        {
                            BlockId = command.BlockId,
                            Type = Enum.Parse<EvaluateGroupBlock.Command.GroupType>(parameters[BlockParameterEntity.ParameterKey.GroupType])
                        });
                    case BlockEntity.BlockType.Change:
                        ///TODO: Execute EvaluateChangeBlock command
                        throw new NotImplementedException();
                    case BlockEntity.BlockType.Value:
                        ///TODO: Execute EvaluateValueBlock command
                        throw new NotImplementedException();
                }

                return false;
            }
        }
    }
}
