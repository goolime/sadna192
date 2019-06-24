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
        List<string> LastNotificationsList { get; set; }
        public NotificationsAlerter(string ip)
        {
            IP = ip;
            LastNotificationsList = new List<string>();
        }

        public bool AlertUser(string message)
        {
            LastNotificationsList.Add(message);
            ShopHub.GlobalContext.Clients.Client(UsersInfo.IPToConnectionID[IP]).SendAsync("ReceiveMessage", message);
            return true;
        }

        public List<string> LastNotifications()
        {
            return LastNotificationsList;
        }
    }
}
