﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sadna192
{
    public interface Alerter
    {
        bool AlertUser(string messege);

        List<string> LastNotifications();
    }
}