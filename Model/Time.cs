using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_resolver.Model
{
    internal class Time
    {
        public string time_str;
       public string Time_str {
            get { 
            
                return "Czas "+time_str;
            }
            set { time_str = value; }
        
        } 
        DateTime currentDateTime;
       public Time() { 
            currentDateTime = DateTime.Now;
        }

        DateTime teraz;
       public int get_time() { 
            teraz = DateTime.Now;


            return DateTime.Compare(currentDateTime, teraz);
        }
        
    }
}
