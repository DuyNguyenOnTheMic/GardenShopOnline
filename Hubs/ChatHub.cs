using Microsoft.AspNet.SignalR;

namespace GardenShopOnline.Hubs
{
    public class ChatHub : Hub
    {
        public static void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.addNewMessageToPage(name, message);
        }
    }
}