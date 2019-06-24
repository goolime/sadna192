using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sadna192;

namespace WebApplication1.Models
{
    public class SessionControler
    {
        ///Todo Remove old sessions
        private static Dictionary<string, I_User_ServiceLayer> Sessions = new Dictionary<string, I_User_ServiceLayer>();

        public static I_User_ServiceLayer GetSession(string ip)
        {
            if (!Sessions.ContainsKey(ip))
            {
                I_ServiceLayer tmp_sl = new sadna192.ServiceLayer();
                Sessions[ip] = tmp_sl.Connect(null);
            }
            return Sessions[ip];
        }

    }
}
