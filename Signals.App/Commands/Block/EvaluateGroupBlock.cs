using MediatR;
using Signals.App.Database;
using Signals.App.Services;

namespace Signals.App.Commands.Block
{
    public class EvaluateGroupBlock
    {
        public class Command : IRequest<bool>
        {
            public Guid BlockId { get; set; }
            public GroupType Type { get; set; }

            public enum GroupType
            {
                And,
                Or
            }
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
                var childrenBlocks = SignalsContext.Blocks
                    .Where(x => x.ParentBlockId == command.BlockId)
                    .ToList();

                var isAnd = command.Type == Command.GroupType.And;

                foreach (var block in childrenBlocks)
                {
                    var isBlockSucceded = await CommandService.Execute(new EvaluateBlock.Command { BlockId = block.Id });

                    if (isAnd)
                    {
                        if (!isBlockSucceded)
                            return false;
                    }
                    else
                    {
                        if (isBlockSucceded)
                            return true;
                    }
                }

                return isAnd;
            }
        }
    }
}
