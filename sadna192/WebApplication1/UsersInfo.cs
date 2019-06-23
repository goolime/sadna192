using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class UsersInfo
    {
        public static readonly ConcurrentDictionary<string, string> ConIDToName = new ConcurrentDictionary<string, string>();



    }
}
