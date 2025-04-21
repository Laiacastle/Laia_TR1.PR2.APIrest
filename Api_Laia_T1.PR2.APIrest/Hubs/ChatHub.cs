using Microsoft.AspNetCore.SignalR;

namespace Api_Laia_T1.PR2.APIrest.Hubs
{
    public class ChatHub : Hub
    {
        //metode invocable desde el client
        public async Task SendMessage(string user, string message)
        {
            //envia el missatge a tots el s clients
            await Clients.All.SendAsync("ReciveMessage", user, message);
        }
    }
}
