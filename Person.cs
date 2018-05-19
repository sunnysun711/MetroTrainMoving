using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_20180412
{
    class Person
    {
        public Station start;
        public Station stop;
        public DateTime arrive;
        public DateTime depart;
        public Che che;

        public Person(Station staStart, Station staStop, DateTime dtArrive,DateTime dtdepart)
        {
            start = staStart;
            stop = staStop;
            arrive = dtArrive;
            depart = dtdepart;
            if(start.location.X < stop.location.X)//Downline
            {

            }else if(start.location.X > stop.location.X)//Upline
            {

            }
        }
    }
}
