using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApp_20180412
{
    
    class Che
    {
        public PictureBox pictureBox = new PictureBox();
        ToolTip ttp = new ToolTip();
        public Label label = new Label();
        public Point ptStart;
        public string cheNum;
        public DateTime[] dateTimes=new DateTime[44];

        //上下行列车的Y坐标常量
        const int DOWNY = 90;
        const int UPY = 111;

        public Che(string carnum, Point pstart, List<DateTime> dts)
        {
            ptStart = pstart;
            cheNum = carnum.Substring(0,3) + "10" +carnum.Substring(3,2) ;
            for(int i=0;i<44;i++)
            {
                dateTimes[i] = dts[i];
            }
            
            pictureBox.Location = pstart;
            label.Location = new Point(pstart.X, pstart.Y + 15);
            label.Text = "车次:" + cheNum;
            label.AutoSize = true;
            pictureBox.Image = Image.FromFile("火车头.png");
            pictureBox.Size = new Size(10, 10);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            //车次和信息显示
            ttp.InitialDelay = 200;
            ttp.AutoPopDelay = 10 * 1000;
            ttp.ReshowDelay = 200;
            ttp.ShowAlways = true;
            ttp.IsBalloon = true;
            string tipOverwrite = "车次:" + cheNum + "\r\n车载旅客数:" ;
            ttp.SetToolTip(this.pictureBox, tipOverwrite); 

        }

        public void Update(int listNum, DateTime dt, List<Station> sta)
        {
            int x = -100;
            int y = -100;
            double per;
            TimeSpan timeSpan1, timeSpan2;
            Point ptFront, ptRear;
            if (dateTimes[0] < dateTimes[43])//downline
            {
                y = DOWNY;
                if (dt >= dateTimes[0] && dt <= dateTimes[43])//判断是否正在运行
                {
                    for (int i = 0; i < 43; i++)//因为下面比较i和i+1，所以到第43个循环即可结束，否则超出数组长度。
                    {
                        if (dt >= dateTimes[i] && dt <= dateTimes[i + 1])//判断处于时刻表中的哪个阶段。i为偶数：停站；i为奇数：区间。
                        {
                            ptFront = new Point(sta[(i + 1) / 2].location.X, DOWNY);//前方站的坐标。例如升仙湖为sta[0],i=0 或 1的时候可取到升仙湖。
                            ptRear = new Point(sta[(i) / 2].location.X, DOWNY);//后方站的坐标。
                            timeSpan1 = dt - dateTimes[i];//在区间已经运行的时间。
                            timeSpan2 = dateTimes[i + 1] - dateTimes[i];//区间运行时分。
                            per = timeSpan1.TotalSeconds / timeSpan2.TotalSeconds;
                            x = (int)((ptFront.X - ptRear.X) * per + ptRear.X);//坐标=后方站坐标+区间长度*（已运行时间/区间运行时分）
                        }
                    }
                }
            }
            else if (dateTimes[0] > dateTimes[43])//upline
            {
                y = UPY; 
                if (dt <= dateTimes[0] && dt >= dateTimes[43])//判断是否正在运行
                {
                    for (int i = 0; i < 43; i++)//因为下面比较i和i+1，所以到第43个循环即可结束，否则超出数组长度。
                    {
                        if (dt <= dateTimes[i] && dt >= dateTimes[i + 1])//判断处于时刻表中的哪个阶段。i为偶数：停站；i为奇数：区间。
                        {
                            ptFront = new Point(sta[i / 2].location.X, UPY);//前方站坐标。
                            ptRear = new Point(sta[(i + 1) / 2].location.X, UPY);//后方站坐标。
                            timeSpan1 = dt - dateTimes[i + 1];//在区间已经运行的时间。
                            timeSpan2 = dateTimes[i] - dateTimes[i + 1];//区间运行时分。
                            per = timeSpan1.TotalSeconds / timeSpan2.TotalSeconds;
                            x = (int)(ptRear.X - (ptRear.X - ptFront.X) * per);//坐标=前方站坐标-区间长度*（已运行时间/区间运行时分）
                            if (i / 2 == (i + 1) / 2)
                            {
                                x = sta[i / 2].location.X;
                            }
                        }
                    }
                }
            }
            pictureBox.Location = new Point(x, y);
        }



    }
}
