﻿using System.Security.Claims;

namespace API.Identity
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var stringId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int.TryParse(stringId, out var currentUserId);

            return currentUserId;
        }
    }
}
