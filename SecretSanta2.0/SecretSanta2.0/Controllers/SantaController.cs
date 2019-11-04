using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretSanta2._0.Enums;
using SecretSanta2._0.Helpers;
using SecretSanta2._0.Models;
using SecretSanta2._0.Services.Business.Interfaces;

namespace SecretSanta2._0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SantaController : ControllerBase
    {
		private readonly ISantaService _santaService;

		public SantaController(ISantaService santaService)
		{
			this._santaService = santaService;
		}

		[HttpGet("[action]")]
		public async Task<ParticipantsModel> GetParticipants()
		{
			try
			{
				return await _santaService.GetParticipants();
			}
			catch (Exception ex)
			{
				LoggerHelper.Log(ex);
				return new ParticipantsModel();
			}
		}

		//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost("[action]")]
		public async Task<PresentModel> GetSecretSanta(string name)
		{
			try
			{
				return await _santaService.GetSecretSanta(name);
			}
			catch (Exception ex)
			{
				LoggerHelper.Log(ex);
				return new PresentModel();
			}
		}

		//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost("[action]")]
		public async Task<eResponse> JoinTheFun([FromBody] InputModel user)
		{
			try
			{
				return await _santaService.JoinTheFun(user);
			}
			catch (Exception ex)
			{
				LoggerHelper.Log(ex);
				return eResponse.Failure;
			}
		}
	}
}