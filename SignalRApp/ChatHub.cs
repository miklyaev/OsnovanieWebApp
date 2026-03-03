using Microsoft.AspNetCore.SignalR;

namespace SignalRApp
{
    public interface IChat
    {
        public Task Send(string message, string userName);
    }
    public class ChatHub : Hub, IChat
    {
        public async Task Send(string message, string userName)
        {
            await this.Clients.All.SendAsync("Receive", message, userName);
        }
    }
}
