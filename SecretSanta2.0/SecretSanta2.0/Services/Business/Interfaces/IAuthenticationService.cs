using Microsoft.AspNetCore.Http;
using SecretSanta2._0.Models;
using System.Threading.Tasks;

namespace SecretSanta2._0.Services.Business.Interfaces
{
	public interface IAuthenticationService
	{
		Task<dynamic> AuthenticateGoogleToken(TokenModel token, HttpResponse response);
	}
}
