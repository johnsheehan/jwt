﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Extensions.Options;

namespace JWT.Extensions.AspNetCore.Factories
{
    public sealed class DefaultIdentityFactory : IIdentityFactory
    {
        private readonly IOptionsMonitor<JwtAuthenticationOptions> _options;

        public DefaultIdentityFactory(IOptionsMonitor<JwtAuthenticationOptions> options) =>
            _options = options;

        /// <summary>
        /// Creates user's identity from user's claims
        /// </summary>
        /// <param name="payload"><see cref="IDictionary{String,String}" /> of user's claims</param>
        /// <returns><see cref="ClaimsIdentity" /></returns>
        public IIdentity CreateIdentity(IDictionary<string, string> payload)
        {
            var claims = payload.Select(p => new Claim(p.Key, p.Value));
            return _options.CurrentValue.IncludeAuthenticationScheme ?
                new ClaimsIdentity(claims, JwtAuthenticationDefaults.AuthenticationScheme) :
                new ClaimsIdentity(claims);
        }
    }
}