using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Bussiness.SignalR.Hubs
{
    public class ConnectionHub : Hub
    {
        public string GetConnectionId() => Context.ConnectionId;

        public async Task Send(string userId, string message)
        {
            await Clients.Client(userId).SendAsync("ReceiveMessage", message);
        }
    }
}
