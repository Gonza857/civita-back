using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CivitaBack.Logica.Hubs
{
    public class EventoHub : Hub
    {
        public async Task UnirseAPartida(string partidaId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, partidaId);
        }

        public async Task SalirDePartida(string partidaId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, partidaId);
        }
    }
}
