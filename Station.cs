using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp_20180412
{
    class Station
    {
        public int num;
        public string name;
        public Point location;
        public int capacity;
        public Label label= new Label();

        public Station(int num, string name, Point location, int capacity)
        {
            this.num = num;
            this.location = location;
            this.capacity = capacity;
            this.name = name;
            //站名和信息显示
            label.Location = new Point(location.X - 10, location.Y + 30);
            label.Text = name;
            label.AutoSize = true;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.BackColor = Color.Black;
            label.ForeColor = Color.White;
            ToolTip ttpSettings = new ToolTip();
            ttpSettings.InitialDelay = 200;
            ttpSettings.AutoPopDelay = 10 * 1000;
            ttpSettings.ReshowDelay = 200;
            ttpSettings.BackColor = Color.Transparent;
            ttpSettings.ShowAlways = true;
            ttpSettings.IsBalloon = true;
            string tipOverwrite = "站内旅客数:" + "\r\n";
            ttpSettings.SetToolTip(label, tipOverwrite); 
        }

        public int Judge()
        {

            return 0;
            //判断车站人数；显示颜色。
        }

    }
}
