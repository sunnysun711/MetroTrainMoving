using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_20180412
{
    class Time
    {
        public int hour, minute;
        public double second;
        public double time_day;
        public int time_sec;

        public Time(int h,int m,double s)
        {
            hour = h;
            minute = m;
            second = s;
            if (second >= 60)
            {
                int minplus = (int)second / 60;
                second %= 60;
                minute += minplus;
            }
            if (minute >= 60)
            {
                int hourplus = minute / 60;
                minute %= 60;
                hour += hourplus;
            }
            if (hour >= 24)
            {
                hour %= 24;
            }

            time_day = (double)h / 24 + (double)m / 1440 + (double)s / 86400;
            time_sec = h * 3600 + m * 60 + (int)s;
        }

        public void Update(double s)
        {
            time_sec = (int)Math.Round(s * 24 * 3600, 0);
            hour = time_sec / 3600;
            minute = (time_sec % 3600 )/ 60;
            second = time_sec % 60;

        }

        public void Update()
        {
            if (this.second >= 60)
            {
                int minplus = (int)this.second / 60;
                this.second %= 60;
                this.minute += minplus;
            }
            if (this.minute >= 60)
            {
                int hourplus = this.minute / 60;
                this.minute %= 60;
                this.hour += hourplus;
            }
            if (this.hour >= 24)
            {
                this.hour %= 24;
            }

        }

        public string Display()
        {
            Update();
            string value = "";
            if (minute / 10 == 0)
            {
                if ((int)second / 10 == 0)
                {
                    value = this.hour + ":0" + this.minute + ":0" + (int)this.second;
                }
                else
                {
                    value = this.hour + ":0" + this.minute + ":" + (int)this.second;
                }
            }
            else if ((int)second / 10 == 0)
            {
                value = this.hour + ":" + this.minute + ":0" + (int)this.second;

            }
            else
            {
                value = this.hour + ":" + this.minute + ":" + (int)this.second;
            }
            return value;
        }

        public DateTime C2D()
        {
            DateTime dt;
            dt = Convert.ToDateTime("2000-1-1 " + this.Display());
            return dt;
        }

    }
}
