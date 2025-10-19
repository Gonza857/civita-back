using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CivitaBack.Logica.Hubs
{
    public class CicloHub : Hub
    {
        public async Task UnirseAPartida(string partidaId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, partidaId.ToString());
        }

        public async Task SalirDePartida(int partidaId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, partidaId.ToString());
        }
    }
}
