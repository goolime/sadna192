using Microsoft.AspNetCore.SignalR;
using sadna192;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Hubs;

namespace WebApplication1
{
    public class NotificationsAlerter : Alerter
    {
        private string IP;
        public NotificationsAlerter(string ip)
        {
            IP = ip;
        }

        public bool AlertUser(string message)
        {
            ShopHub.GlobalContext.Clients.Client(UsersInfo.IPToConnectionID[IP]).SendAsync("ReceiveMessage", message);
            return true;
        }
    }
}
