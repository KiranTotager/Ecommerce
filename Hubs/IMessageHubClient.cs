namespace ECommerce.Hubs
{
    public interface IMessageHubClient
    {
        public Task SendConnected(string message);
        public Task SendOffers(List<string> offers);
       
    }
}
