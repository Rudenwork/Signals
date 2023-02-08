using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Signals.App.Controllers.Models;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Database.Entities.Stages;
using Signals.App.Extensions;

namespace Signals.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SignalsController : ControllerBase
    {
        private SignalsContext SignalsContext { get; }

        public SignalsController(SignalsContext signalsContext)
        {
            SignalsContext = signalsContext;
        }

        ///TODO: Implement
        [HttpGet]
        public ActionResult<List<SignalModel_X>> Get([FromQuery] SubsetModel subset, [FromQuery] SignalModel_X.Read.Filter filter) 
        {
            throw new NotImplementedException();

            var query = SignalsContext.Signals.AsQueryable();

            var executionsQuery = SignalsContext.Executions.AsQueryable();

            var test = from signal in query
                       join execution in executionsQuery on signal.Id equals execution.SignalId into joined
                       from sub in joined.DefaultIfEmpty()
                       select new
                       {
                           //Signal,
                           signal.Id,
                           signal.UserId,
                           signal.Name,
                           signal.Schedule,
                           signal.IsDisabled,
                           signal.Stages,

                           HasExecution = sub.Id
                       };

            //if (filter.Status is not null)
            //{
            //    query = filter.Status switch
            //    {
            //        ///TODO: !Disabled && HasExecution
            //        SignalModel.Read.Filter.StatusEnum.InProgress => query.Where(),
            //        ///TODO: !Disabled && !HasExecution
            //        SignalModel.Read.Filter.StatusEnum.Scheduled => query.Where(),
            //        ///TODO: Disabled
            //        SignalModel.Read.Filter.StatusEnum.Disabled => query.Where(x => x.IsDisabled)
            //    };
            //}

            //if (filter.Name is not null)
            //    query = query.Where(x => x.Name.Contains(filter.Name));

            /////TODO: x.Schedule.Contains --> x.Schedule.StartsWith() ?
            //if (filter.Schedule is not null)
            //    query = query.Where(x => x.Schedule.Contains(filter.Schedule));

            //var result = query
            //    .Where(x => x.UserId == User.GetId())
            //    .Subset(subset.Offset, subset.Limit)
            //    .Select(x => AdaptToModel(x))
            //    .ToList();

            var result = test;
                //.Adapt<List<SignalModel.Read>>(); 

            return Ok(result);
        }

        ///TODO: Implement
        [HttpGet("{id}")]
        public ActionResult<SignalModel_X.Read> Get(Guid id)
        {
            throw new NotImplementedException();

            var entity = SignalsContext.Signals.Find(id);

            if (entity == null)
                return NoContent();

            ///TODO: if not same id but is admin OK (Role in claims?)
            if (entity.UserId != User.GetId())
                return Forbid();

            var result = entity.Adapt<SignalModel_X.Read>();

            return Ok(result);
        }

        ///TODO: Implement
        [HttpPost]
        public async Task<ActionResult<SignalModel>> Post(SignalModel.Create model)
        {
            return Ok();
        }
        
        ///TODO: Implement
        private static SignalModel_X.Read AdaptToModel(SignalEntity entity)
        {
            throw new NotImplementedException();
        }

        private static StageModel_X AdaptToModel(StageEntity entity) =>
        entity switch
        {
            ConditionStageEntity => entity.Adapt<StageModel_X.Condition.Read>(),
            WaitingStageEntity => entity.Adapt<StageModel_X.Waiting.Read>(),
            NotificationStageEntity => entity.Adapt<StageModel_X.Notification.Read>()
        };
    }
}
