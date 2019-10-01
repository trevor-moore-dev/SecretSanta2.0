using Google.Apis.Auth;
using SecretSanta2._0.Models;
using System;
using System.Threading.Tasks;
using SecretSanta2._0.Helpers;
using Microsoft.AspNetCore.Http;

namespace SecretSanta2._0.Services.Business
{
	public class AuthenticationService : IAuthenticationService
	{
		public async Task<dynamic> AuthenticateGoogleToken(TokenModel token, HttpResponse response)
		{
			try
			{
				var payload = await GoogleJsonWebSignature.ValidateAsync(token.tokenId, new GoogleJsonWebSignature.ValidationSettings());
				var jwt = TokenHelper.GenerateToken(payload.Email);

				LoggerHelper.Log(payload.ExpirationTimeSeconds.ToString());
				CookieHelper.AddCookie(response, "User-Email", payload.Email);
				CookieHelper.AddCookie(response, "Authorization-Token", jwt.token);

				return jwt;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
