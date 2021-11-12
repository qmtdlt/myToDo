using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace todo.Api.Hubs
{
    [EnableCors("cors")]
    public class ChatHub : Hub
    {
        [EnableCors("cors")]
        public async Task hand_shake()
        {
            await Clients.Caller.SendAsync("connect_echo", "链接建立成功");
        }

        [EnableCors("cors")]
        public async Task guang_bo(string msg)
        {
            await Clients.Caller.SendAsync("guang_bo", msg);
        }
    }
}
