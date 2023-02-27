using Microsoft.AspNetCore.SignalR;

namespace SignalRApp
{
    public class ChatHub : Hub
    {
        public async Task Send(string message, string userName)
        {
            await this.Clients.All.SendAsync("Receive", message, userName);
        }
    }
}
