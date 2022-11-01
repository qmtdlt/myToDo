using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace todo.Api.Hubs
{
    [EnableCors("cors")]
    public class ChatHub : Hub
    {
        [EnableCors("cors")]
        public async Task guang_bo(string msg)
        {
            await Clients.AllExcept(this.Context.ConnectionId).SendAsync("guang_bo", this.Context.ConnectionId.Substring(0,6) + "\t" + msg + " \n");
        }
    }
}
