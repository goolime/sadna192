using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace sadna192
{
    public class Sadna192Exception : Exception
    {
        public static Logger log = new Logger("Event Log");
    

        public Sadna192Exception(String exc , String classLoc , String func ) 
        {
            log.Add("ERROR => " + exc + ".   <" + classLoc + " : " + func +">");
            throw new Exception(exc); 
        }

        public static void AddToEventLog (String str)
        {
            log.Add(str);
        }
 
    }
}
