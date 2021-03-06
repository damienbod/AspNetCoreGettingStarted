﻿using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreGettingStarted.Features.Security
{
    public class GetJwtTokenQuery
    {
        public class Request: IRequest<Response> {
            public string Username { get; set; }
        }

        public class Response {
            public string AccessToken { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public Handler(IOptions<AuthConfiguration> authConfiguration)
            {
                _authConfiguration = authConfiguration;
            }
            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var now = DateTime.UtcNow;
                var nowDateTimeOffset = new DateTimeOffset(now);

                var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, request.Username),
                    new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, nowDateTimeOffset.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                };

                var jwt = new JwtSecurityToken(
                    issuer: _authConfiguration.Value.JwtIssuer,
                    audience: _authConfiguration.Value.JwtAudience,
                    claims: claims,
                    notBefore: now,
                    expires: now.AddMinutes(_authConfiguration.Value.ExpirationMinutes),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authConfiguration.Value.JwtKey)), SecurityAlgorithms.HmacSha256));

                return Task.FromResult(new Response()
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt)
                });
            }

            private readonly IOptions<AuthConfiguration> _authConfiguration;
        }
    }
}
