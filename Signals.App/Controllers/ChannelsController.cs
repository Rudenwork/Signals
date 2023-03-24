﻿using Mapster;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Signals.App.Controllers.Models;
using Signals.App.Core.Execution;
using Signals.App.Core.Notification;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Database.Entities.Channels;
using Signals.App.Database.Entities.Stages;
using Signals.App.Extensions;
using System.Data;
using System.Security.Cryptography.Xml;

namespace Signals.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChannelsController : ControllerBase
    {
        private SignalsContext SignalsContext { get; }
        private IMediator Mediator { get; }

        public ChannelsController(SignalsContext signalsContext, IMediator mediator)
        {
            SignalsContext = signalsContext;
            Mediator = mediator;
        }

        [HttpGet]
        public ActionResult<List<ChannelModel.Read>> Get([FromQuery] SubsetModel subset, [FromQuery] ChannelModel.Read.Filter filter)
        {
            var query = SignalsContext.Channels.AsQueryable();

            if (filter.Type is not null)
            {
                query = filter.Type switch
                {
                    ChannelModel.Read.Filter.TypeEnum.Email => query.Where(x => x is EmailChannelEntity),
                    ChannelModel.Read.Filter.TypeEnum.Telegram => query.Where(x => x is TelegramChannelEntity)
                };
            }

            if (filter.Description is not null)
                query = query.Where(x => EF.Functions.ILike(x.Description, $"%{filter.Description}%"));

            if (filter.IsVerified is not null)
                query = query.Where(x => x.IsVerified == filter.IsVerified.Value);

            if (filter.Address is not null)
                query = query.Where(x => EF.Functions.ILike((x as EmailChannelEntity).Address, $"%{filter.Address}%"));

            if (filter.Username is not null)
                query = query.Where(x => EF.Functions.ILike((x as TelegramChannelEntity).Username, $"%{filter.Username}%"));

            var result = query
                .Where(x => x.UserId == User.GetId())
                .Subset(subset.Offset, subset.Limit)
                .Select(x => AdaptToModel(x))
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<ChannelModel.Read> Get(Guid id)
        {
            var entity = SignalsContext.Channels.Find(id);

            if (entity == null)
                return NoContent();

            if (entity.UserId != User.GetId())
                return Forbid();

            var result = AdaptToModel(entity);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ChannelModel.Read>> Post(ChannelModel.Create model)
        {
            switch (model)
            {
                case ChannelModel.Create.Email emailModel:
                    if (SignalsContext.Channels.Any(x => (x as EmailChannelEntity).Address.ToLower() == emailModel.Address.ToLower()))
                    {
                        ModelState.AddModelError(nameof(emailModel.Address), "Already created");
                        return ValidationProblem();
                    }
                    break;
                case ChannelModel.Create.Telegram telegramModel:
                    if (SignalsContext.Channels.Any(x => (x as TelegramChannelEntity).Username.ToLower() == telegramModel.Username.ToLower()))
                    {
                        ModelState.AddModelError(nameof(telegramModel.Username), "Already created");
                        return ValidationProblem();
                    }
                    break;
            }

            ChannelEntity entity = model switch
            {
                ChannelModel.Create.Email => model.Adapt<EmailChannelEntity>(),
                ChannelModel.Create.Telegram => model.Adapt<TelegramChannelEntity>()
            };

            entity.UserId = User.GetId();
            entity.Code = GenerateCode();

            if (entity is EmailChannelEntity emailEntity)
                await SendVerificationEmail(emailEntity);

            SignalsContext.Channels.Add(entity);
            SignalsContext.SaveChanges();

            var result = AdaptToModel(entity);

            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ChannelModel.Read>> Patch(Guid id, ChannelModel.Update model)
        {
            var entity = SignalsContext.Channels.Find(id);

            if (entity is null)
                return NoContent();

            if (entity.UserId != User.GetId())
                return Forbid();

            switch (model)
            {
                case ChannelModel.Update.Email emailModel:
                    if (SignalsContext.Channels.Any(x => x.Id != id && (x as EmailChannelEntity).Address.ToLower() == emailModel.Address.ToLower()))
                    {
                        ModelState.AddModelError(nameof(emailModel.Address), "Already created");
                        return ValidationProblem();
                    }
                    break;
                case ChannelModel.Update.Telegram telegramModel:
                    if (SignalsContext.Channels.Any(x => x.Id != id && (x as TelegramChannelEntity).Username.ToLower() == telegramModel.Username.ToLower()))
                    {
                        ModelState.AddModelError(nameof(telegramModel.Username), "Already created");
                        return ValidationProblem();
                    }
                    break;
            }

            ChannelEntity altEntity = model switch
            {
                ChannelModel.Update.Email => new EmailChannelEntity { Address = (model as ChannelModel.Update.Email).Address },
                ChannelModel.Update.Telegram => new TelegramChannelEntity { Username = (model as ChannelModel.Update.Telegram).Username }
            };

            if (entity.GetType() != altEntity.GetType())
            {
                altEntity.UserId = entity.UserId;
                altEntity.Description = entity.Description;
                altEntity.Code = entity.Code;

                SignalsContext.Channels.Add(altEntity);

                var stages = SignalsContext.Stages
                    .Where(x => (x as NotificationStageEntity).ChannelId == id)
                    .Cast<NotificationStageEntity>()
                    .ToList();

                var signalIds = stages
                    .Select(x => x.SignalId)
                    .ToList();

                var executions = SignalsContext.Executions
                    .Where(x => signalIds.Contains(x.SignalId))
                    .ToList();

                foreach (var execution in executions)
                {
                    await Mediator.Publish(new Stop.Message { ExecutionId = execution.Id }); 
                }

                foreach (var stage in stages)
                {
                    stage.ChannelId = altEntity.Id;
                }

                SignalsContext.SaveChanges();
                SignalsContext.Channels.Remove(entity);
                entity = altEntity;

                foreach (var execution in executions)
                {
                    await Mediator.Publish(new Start.Message { SignalId = execution.SignalId });
                }
            }

            var shouldReset = model switch
            {
                ChannelModel.Update.Email emailModel => emailModel.Address is not null,
                ChannelModel.Update.Telegram telegramModel => telegramModel.Username is not null
            };

            if (shouldReset)
            {
                entity.IsVerified = false;
                entity.Code = GenerateCode();
            }

            model.Adapt(entity, model.GetType(), entity.GetType());

            if (shouldReset && entity is EmailChannelEntity emailEntity)
                await SendVerificationEmail(emailEntity);

            SignalsContext.Channels.Update(entity);
            SignalsContext.SaveChanges();

            var result = AdaptToModel(entity);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var entity = SignalsContext.Channels.Find(id);

            if (entity is null)
                return NoContent();

            if (entity.UserId != User.GetId())
                return Forbid();

            SignalsContext.Channels.Remove(entity);
            SignalsContext.SaveChanges();

            return Ok();
        }

        [HttpPost("{id}/[action]")]
        public async Task<ActionResult> Verify(Guid id, ChannelModel.Verify model)
        {
            var entity = SignalsContext.Channels.Find(id);

            if (entity is null)
                return NoContent();

            if (entity.UserId != User.GetId())
                return Forbid();

            if (entity.IsVerified)
            {
                ModelState.AddModelError(nameof(entity.IsVerified), "Already verified");
                return ValidationProblem();
            }

            if (model.Code != entity.Code)
            {
                ModelState.AddModelError(nameof(model.Code), "Invalid");
                return ValidationProblem();
            }

            entity.IsVerified = true;

            SignalsContext.Channels.Update(entity);
            SignalsContext.SaveChanges();

            return Ok();
        }

        private static string GenerateCode() => Random.Shared.Next(1000, 10000).ToString();

        ///TODO: Add derived types mapping configuration
        private static ChannelModel.Read AdaptToModel(ChannelEntity entity) => entity switch
        {
            EmailChannelEntity => entity.Adapt<ChannelModel.Read.Email>(),
            TelegramChannelEntity => entity.Adapt<ChannelModel.Read.Telegram>()
        };

        private async Task SendVerificationEmail(EmailChannelEntity entity)
        {
            await Mediator.Send(new SendEmailNotification.Request
            {
                Address = entity.Address,
                Topic = "Signals Verification Code",
                Text = $"Verification Code: {entity.Code}"
            });
        }
    }
}
