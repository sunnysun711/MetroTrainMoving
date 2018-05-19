using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;


namespace WindowsFormsApp_20180412
{
    public partial class Form1 : Form
    {

        //定义初始运营时间
        private Time now = new Time(5,47,10);
        //定义列车List和车站List
        private List<Che> chesD = new List<Che>();
        private List<Che> chesU = new List<Che>();
        private List<Station> stations = new List<Station>();
        //定义时间步长
        public double timeStep = 0;
        //上下行线路的Y坐标常量
        const int DOWNY = 95;
        const int UPY = 105;
        //Access
        private OleDbConnection oleDbConnect = new OleDbConnection();
        string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + Application.StartupPath + @"\Table.mdb";
        //定义datetime中的年月日
        const string YMD = "2000-1-1 ";
        //定义欢迎界面
        Form2 form2;

        //窗体构造函数
        public Form1()
        {
            // connect to Access via connection string.
            oleDbConnect.ConnectionString = connectionString;

            //打开form2加载窗口
            form2 = new Form2();
            form2.Show();


            InitializeComponent();
            
            //display system time.
            timerSystemtime.Enabled = true;

            //显示默认开始时间。
            labelTimeNow.Text = "运营时间：" + now.Display();


        }

        //画线路和车站
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            Pen whitep = new Pen(Color.White, 1);
            for (int i = 1; i <= stations.Count; i++)
            {
                g.DrawRectangle(whitep, i * 60 - 20, 100, 10, 10);
                //区间上下行线路
                if (i == stations.Count)
                {
                    g.DrawLine(whitep, 45, 100, i * 60 - 20, 100);
                    g.DrawLine(whitep, 45, 110, i * 60 - 20, 110);
                }
            }

        }

        //窗体加载函数
        private void Form1_Load(object sender, EventArgs e)
        {


            //连接Access
            oleDbConnect.Open();
            OleDbCommand oleDbCmd = new OleDbCommand();
            oleDbCmd.Connection = oleDbConnect;

            //定义需要用到的变量，以便存入ches数组中。
            string carnum = "";
            List<DateTime> dateTimes = new List<DateTime>();
            //下行存入chesD数组
            string SQLdownline = "SELECT * FROM 下行 ";
            oleDbCmd.CommandText = SQLdownline;
            OleDbDataReader readerDL = oleDbCmd.ExecuteReader();
            while (readerDL.Read())
            {
                for (int i = 0; i < 45; i++)
                {
                    if (i == 0)
                    {
                        carnum = readerDL[i].ToString();
                    }
                    else
                    {
                        dateTimes.Add(C2D(readerDL[i].ToString()));
                    }
                }
                chesD.Add(new Che(carnum, new Point(-100, -100), dateTimes));
                dateTimes.RemoveRange(0, dateTimes.Count);
            }
            readerDL.Close();

            //上行存入cheU数组
            string SQLupline = "SELECT * FROM 上行 ";
            oleDbCmd.CommandText = SQLupline;
            OleDbDataReader readerUL = oleDbCmd.ExecuteReader();
            while (readerUL.Read())
            {
                for (int i = 0; i < 45; i++)
                {
                    if (i == 0)
                    {
                        carnum = readerUL[i].ToString();
                    }
                    else
                    {
                        dateTimes.Add(C2D(readerUL[i].ToString()));
                    }
                }
                chesU.Add(new Che(carnum, new Point(-100, -100), dateTimes));
                dateTimes.RemoveRange(0, dateTimes.Count);
            }
            readerUL.Close();


            //从数据库加车站信息。
            string SQLstation = "SELECT * FROM station ORDER BY Num ASC";
            oleDbCmd.CommandText = SQLstation;
            OleDbDataReader readerStation = oleDbCmd.ExecuteReader();
            int stationNum, locX, locY, capacity;
            string stationName;
            while (readerStation.Read())
            {
                stationNum = Convert.ToInt32(readerStation[0]);
                stationName = Convert.ToString(readerStation[1]);
                locX = Convert.ToInt32(readerStation[2]);
                locY = Convert.ToInt32(readerStation[3]);
                capacity = Convert.ToInt32(readerStation[4]);
                Station station = new Station(stationNum, stationName, new Point(locX, locY), capacity);
                stations.Add(station);
                Controls.Add(station.label);

            }

            //断开Access连接。
            oleDbConnect.Close();

            //加picturebox
            foreach (Che che in chesD)
            {
                Controls.Add(che.pictureBox);
            }
            foreach (Che che in chesU)
            {
                Controls.Add(che.pictureBox);
            }



        }

        //更新时间步长、更新车的位置
        private void timer_Tick(object sender, EventArgs e)
        {
            //修改步长
            timeStep = Convert.ToDouble(comboBoxSpeed.Text) * (double)timer.Interval / 1000;

            //更新时间并显示
            now.second += timeStep;
            now.Update();
            labelTimeNow.Text = "运营时间：" + now.Display();
            int i = 0;

            
            //移动列车
            for (i = 0; i < 263; i++)
            {
                chesD[i].Update(i, now.C2D(), stations);
                chesU[i].Update(i, now.C2D(), stations);
            }

        }
        
        //右上角系统时间显示
        private void timerSystemtime_Tick(object sender, EventArgs e)
        {
            //显示系统时间
            this.labelSystemTime.Text = DateTime.Now.ToString();
        }
        
        //将8:00:00格式的string型转成Datetime型
        private DateTime C2D(string s)
        {
            //s must in format like hh:mm:ss
            DateTime result;
            result = Convert.ToDateTime(YMD + s);
            return result;
        }

        //退出按钮
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        //任意位置拖拽界面移动窗口
        private Point offset;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseButtons.Left != e.Button) return;

            Point cur = this.PointToScreen(e.Location);
            offset = new Point(cur.X - this.Left, cur.Y - this.Top);
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseButtons.Left != e.Button) return;

            Point cur = MousePosition;
            this.Location = new Point(cur.X - offset.X, cur.Y - offset.Y);

        }
        
        //最大化和还原按钮
        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (btnMaximize.Text == "最大化")
            {
                this.WindowState = FormWindowState.Maximized;
                btnMaximize.Text = "还原";
            }else if(btnMaximize.Text == "还原")
            {
                this.WindowState = FormWindowState.Normal;
                btnMaximize.Text = "最大化";
            }
        }

        //开始和暂停按钮
        private void btnStartStop_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            if (btnStartStop.Text == "暂停")
            {
                timer.Stop();
                btnStartStop.Text = "开始";
            }
            else
            {
                if (textBoxHour.Text != "" && textBoxMinute.Text != "" && textBoxSecond.Text != "")
                {
                    now.hour = Convert.ToInt32(textBoxHour.Text);
                    now.minute = Convert.ToInt32(textBoxMinute.Text);
                    now.second = Convert.ToInt32(textBoxSecond.Text);
                }
                now.Update();
                labelTimeNow.Text = "运营时间：" + now.Display();
                timer.Start();
                btnStartStop.Text = "暂停";
            }
        }

        private void timerOpenForm2_Tick(object sender, EventArgs e)
        {
            form2.Dispose();       //关闭SplashScreen
            this.WindowState = FormWindowState.Normal;      //打开主界面
            this.timerOpenForm2.Enabled = false;
        }

    }

}
