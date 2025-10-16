using Microsoft.AspNetCore.SignalR;

namespace ECommerce.Hubs
{
    public class MessageHubClient : Hub<IMessageHubClient>
    {
        public override async Task OnConnectedAsync()
        {
           await Clients.Caller.SendConnected("you are connected to the server");
        }
        public async Task SendOffers(List<string> offers)
        {
            await Clients.All.SendOffers(offers);
        }
        public async Task SendOffers1()
        {
            await Clients.All.SendOffers(new List<string> { "50 % off", "buy one get one free", "deepavali offer" });
        }
    }
}
