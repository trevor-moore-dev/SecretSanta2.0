using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SecretSanta2._0.Helpers;
using SecretSanta2._0.Services.Business.Interfaces;

namespace SecretSanta2._0.Services.SignalR
{
    public class SantaHub : Hub
    {
        private readonly ISantaService _santaService;

        public SantaHub(ISantaService santaService)
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