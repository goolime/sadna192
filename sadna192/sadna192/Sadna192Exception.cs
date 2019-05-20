using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace sadna192
{
    class Sadna192Exception : Exception
    {
        public static Logger errlog = new Logger("Error Log");
    

        public Sadna192Exception(String exc , String classLoc , String func ) 
        {
            errlog.Add(System.DateTime.Now.ToString() + " : " + exc + ".   <" + classLoc + " : " + func +">");
            throw new Exception(exc); 
        }
 
    }
}
