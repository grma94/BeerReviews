using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace BeerReviews.WebApi.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetIsAdmin(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("isAdmin");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}