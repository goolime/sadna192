using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Hubs
{
    public class ShopHub : Hub
    {

        public static readonly string[] reasons = new string[]
        {
            "close_store",
            "reopen_store",
            "delete_store",
            "buy_product",
            "account_deleted",
            "higher_bead",
            "win_auction",
            "message_sent",
            "lottery_ended_price_not_reached",
            "lottery_ended_win",
            "lottery_ended_lose"
        };

        public static IHubContext<ShopHub> GlobalContext { get; private set; }
        public ShopHub(IHubContext<ShopHub> ctx)
        {
            GlobalContext = ctx;
        }

        public async Task SendMessage(string userIP, string message)
        {
            if (UsersInfo.IPToConnectionID.Keys.Contains(userIP))
            {
                await Clients.Client(UsersInfo.IPToConnectionID[userIP]).SendAsync("ReceiveMessage", message);
            }
            else
            {

            }
        }

        public async Task SendMessage2(string message)
        {
           await Clients.Caller.SendAsync("ReceiveMessage", message);
        }

        public async Task SentToClient(string userIP, string message, string reason)
        {
            if (UsersInfo.IPToConnectionID.Keys.Contains(userIP))
            {
                
                await Clients.Client(UsersInfo.IPToConnectionID[userIP]).SendAsync("ReceiveMessage", message, reason);
            }
            else
            {

            }
        }

        private string GetIp()
        {
            return GetHttpContextExtensions.GetHttpContext(this.Context).Connection.RemoteIpAddress.ToString();
        }

        public override async Task OnConnectedAsync()
        {
            UsersInfo.IPToConnectionID.TryAdd(GetIp(),Context.ConnectionId);
            //await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            UsersInfo.IPToConnectionID.TryRemove(GetIp(),out string b);
            await base.OnDisconnectedAsync(exception);

        }
    }
}
