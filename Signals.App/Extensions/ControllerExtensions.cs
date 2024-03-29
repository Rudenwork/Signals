﻿using Signals.App.Identity;
using System.Security.Claims;

namespace Signals.App.Extensions
{
    public static class ControllerExtensions
    {
        public static Guid GetId(this ClaimsPrincipal principal)
        {
            return Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        public static bool IsAdmin(this ClaimsPrincipal principal)
        {
            return principal.IsInRole(IdentityRoles.Admin);
        }

        public static IQueryable<T> Subset<T>(this IQueryable<T> query, int? offset, int? limit)
        {
            if (offset is not null)
                query = query.Skip(offset.Value);

            if (limit is not null)
                query = query.Take(limit.Value);

            return query;
        }
    }
}
