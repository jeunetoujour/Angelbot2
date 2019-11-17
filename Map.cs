using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
//using System.Drawing;
using AngelRead;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;


namespace AngelBot
{
    public struct FloatPoint
    {

        float _X;
        float _Y;

        public float X
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
            }
        }

        public float Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
            }
        }


    }

    public partial class Map : Form
    {


       
        //Player f1.pc;
        //Point[] apt = new Point();
        Form1 f1 = (Form1)Application.OpenForms["Form1"];
        float oldX, oldY;
        float minX, maxX, minY, maxY, maxX2, maxY2, minX2, minY2 = 0;
        //float currentx, currenty;
        Pen p = new Pen(Color.Black, 15);

        GraphicsPath linePath = new GraphicsPath();
        public List<PointF> path = new List<PointF>();
        public List<PointF> pathTest = new List<PointF>();
        PointF tempPoint;
        PointF oldPoint;
        int waypointInt = 1;
        //int waypointCount = 0;
        //bool start = true;
        bool isRecorded = true;
        bool refreshed = false;
        // Render render = new Render();

        Pen pathPen = new Pen(Brushes.Coral);
        Pen playerPen = new Pen(Brushes.Red);


        public Map()
        {
            /*InitializeComponent();
            AionProcess.Open();
            f1.pc = new Player();
            */
            InitializeComponent();
            ResizeRedraw = true;
           
            oldX = f1.pc.X;
            oldY = f1.pc.Y;
            oldPoint.X = 0;
            oldPoint.Y = 0;
            pathPen.Width = 5;
            playerPen.Width = 4;


            timer1.Enabled = true;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           if (Tools.Distance2D(f1.pc.X, f1.pc.Y, oldX, oldY) < waypointInt & refreshed)
            {
                return;
            }

            if (isRecorded)
            {
                if (Tools.Distance2D(f1.pc.X, f1.pc.Y, oldX, oldY) < waypointInt)
                {
                    return;
                }
                tempPoint.X = f1.pc.X;
                tempPoint.Y = f1.pc.Y;

                //path.Add(tempPoint);

                oldX = tempPoint.X;
                oldY = tempPoint.Y;
            } 
        
            render1.Refresh();
            refreshed = true;
        }     


        private void render1_Paint(object sender, PaintEventArgs e)
        {
            //render1.Height = render1.Height - 22;
            float xOrigin = (e.ClipRectangle.Width / 2);
            float yOrigin = e.ClipRectangle.Height / 2;
            // label1.Text = render1.Width.ToString() + ", " + render1.Height.ToString() + ":" + e.ClipRectangle.Width.ToString() +", " +  e.ClipRectangle.Height.ToString();


            Graphics g = e.Graphics;

            float tempx, tempy;

            if (f1.waypointlist.Count < 2)
                return;

            minX = f1.waypointlist[0].X;
            minY = f1.waypointlist[0].Y;
            maxX = f1.waypointlist[0].X;
            maxY = f1.waypointlist[0].Y;

            for (int i = 0; i < f1.waypointlist.Count; i++)
            {
                tempx = f1.waypointlist[i].X;
                tempy = f1.waypointlist[i].Y;

                if (tempx < minX)
                    minX = tempx;
                if (tempy < minY)
                    minY = tempy;
                if (tempx > maxX)
                    maxX = tempx;
                if (tempy > maxY)
                    maxY = tempy;
            }

            tempx = f1.pc.X;
            tempy = f1.pc.Y;

            if (tempx < minX)
                minX = tempx;
            if (tempy < minY)
                minY = tempy;
            if (tempx > maxX)
                maxX = tempx;
            if (tempy > maxY)
                maxY = tempy;
            minY -= 5;
            maxY += 5;
            minX -= 5;
            maxX += 5;

            maxX2 = maxX - minX;
            maxY2 = maxY - minY;

            minX2 = (minX / maxX2) * (e.ClipRectangle.Width);
            minY2 = (minY / maxY2) * (e.ClipRectangle.Height);




            double radians = (System.Math.PI * 3) / 2;
            var newLocation = Tools.RotateAroundPoint2D(tempPoint.X, tempPoint.Y, radians, xOrigin, yOrigin);
            tempPoint.X = newLocation.X;
            tempPoint.Y = newLocation.Y;
            
            Font fonty = new Font("Arial", 13);

            for (int i = 0; i < f1.waypointlist.Count; i++)
            {

                tempPoint.X = (f1.waypointlist[i].X / maxX2) * (e.ClipRectangle.Width);
                tempPoint.X = tempPoint.X - minX2;
                tempPoint.Y = (f1.waypointlist[i].Y / maxY2) * (e.ClipRectangle.Height);
                tempPoint.Y = tempPoint.Y - minY2;
                newLocation = Tools.RotateAroundPoint2D(tempPoint.X, tempPoint.Y, radians, xOrigin, yOrigin);
                tempPoint.X = newLocation.X;
                tempPoint.Y = newLocation.Y;
                //pathTest.Add(tempPoint);
                // linePath.AddLine(oldPoint, tempPoint);
                // g.FillEllipse(Brushes.Blue, tempPoint.X, tempPoint.Y, 5, 5);
                g.DrawLine(pathPen, oldPoint, tempPoint);
                g.FillEllipse(Brushes.Green, tempPoint.X, tempPoint.Y , 13, 13);
                tempPoint.X += 4;
                tempPoint.Y += 4;
                g.DrawString(i.ToString(), fonty, Brushes.Black, tempPoint);
                //linePath.(oldPoint,tempPoint);
                oldPoint = tempPoint;

            }
            tempPoint.X = (f1.pc.X / maxX2) * (e.ClipRectangle.Width);
            tempPoint.X = tempPoint.X - minX2;
            tempPoint.Y = (f1.pc.Y / maxY2) * (e.ClipRectangle.Height);
            tempPoint.Y = tempPoint.Y - minY2;
            newLocation = Tools.RotateAroundPoint2D(tempPoint.X, tempPoint.Y, radians, xOrigin, yOrigin);
            tempPoint.X = newLocation.X;
            tempPoint.Y = newLocation.Y;

            g.FillEllipse(Brushes.Red, tempPoint.X, tempPoint.Y, 10, 10);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void render1_Load(object sender, EventArgs e)
        {

        }


    }
}
