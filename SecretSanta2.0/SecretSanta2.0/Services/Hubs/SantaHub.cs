using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SecretSanta2._0.Helpers;
using SecretSanta2._0.Services.Business.Interfaces;

namespace SecretSanta2._0.Services.Hubs
{
    public class SantaHub : Hub
    {
		//private readonly ISantaService _santaService;
		private readonly ISantaService2 _santaService;

		//public SantaHub(ISantaService santaService)
		public SantaHub(ISantaService2 santaService)
		{
            this._santaService = santaService;
        }

        public async Task GetParticipants()
        {
            try
            {
                var participants = await _santaService.GetParticipants();
                await Clients.All.SendAsync("GetParticipants", participants);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log(ex);
            }
        }
    }
}