using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace многоугольники
{
    public partial class Form1 : Form

    {
        //Draw shape;
        int whatshape, amount;
        bool Dodraw, NotNull, yes, figureGrab;
        List<Draw> versh;
        public Form1()
        {
            InitializeComponent();
            Dodraw = false;
            versh = new List<Draw>();
            whatshape = 0;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
        } 
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (Draw v in versh)
            {
                
                if (v.drag)
                    {
                        v.SetX = e.X - v.Del_x;
                        v.SetY = e.Y - v.Del_y;
                    }
            }
            Refresh();
        }
        private void окружностьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            whatshape = 0;
        }

        private void квадратToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            whatshape = 1;
        }

        private void треугольникToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            whatshape = 2;
        }
        public bool InsideFig(int x, int y, List<Draw> vershin)
        {
            if (versh.Count() > 2)
            {
                List<Draw> pol = new List<Draw>();
                //bool Up = true;
                //bool Down = true;
                Draw circle = new circle(x, y);
                vershin.Add(circle);
                for (int i = 0; i < vershin.Count(); i++)
                {
                    for (int j = i+1; j < vershin.Count(); j++)
                    {
                        bool Up = true;
                        bool Down = true;
                        for (int k = 0; k < vershin.Count(); k++)
                        {
                            if (k != j && k != i && i!=j)
                            {

                                if ((vershin[i].SetY - vershin[j].SetY) * vershin[k].SetX + (vershin[j].SetX - vershin[i].SetX) * vershin[k].SetY + (vershin[i].SetX * vershin[j].SetY - vershin[j].SetX * vershin[i].SetY) >= 0)
                                {
                                    Down = false;
                                }
                                else
                                {
                                    Up = false; 
                                }
                            }
                        }
                        if (Down == true || Up == true)
                        {
                            pol.Add(vershin[i]);
                            pol.Add(vershin[j]);
                        }

                    }
                }
                if (!pol.Contains(circle))
                {
                    vershin.Remove(circle);
                    return true;
                }
                vershin.Remove(circle);
            }
            return false;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            yes = false;
            NotNull = false;
            for (int j = 0; j < versh.Count; j++) NotNull = true;
            if (NotNull)
            { 
                foreach (Draw v in versh)
                {
                    if (v.check(e.X, e.Y))
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            v.Del_x = e.X - v.SetX;
                            v.Del_y = e.Y - v.SetY;
                            v.drag = true;
                            yes = true;
                        }
                        if (e.Button == MouseButtons.Right)
                        {
                            versh.Remove(v);
                            yes = true;
                            Refresh();
                            break;
                        }
                    }
                }
                if (!yes)
                {
                    Dodraw = true;
                    if (InsideFig(e.X, e.Y, versh))
                    {
                        for (int i = 0; i < versh.Count(); i++)
                        {
                            versh[i].Del_x = e.X - versh[i].SetX;
                            versh[i].Del_y = e.Y - versh[i].SetY;
                            versh[i].drag = true;
                        }
                    }   
                    else
                    {
                        Dodraw = true;
                        switch (whatshape)
                        {
                            case 0: versh.Add(new circle(e.X, e.Y)); break;
                            case 1: versh.Add(new square(e.X, e.Y)); break;
                            case 2: versh.Add(new triangle(e.X, e.Y)); break;
                            default: versh.Add(new circle(e.X, e.Y)); break;
                        }
                        Refresh();
                    }
                }
            }
            else
            {
                switch (whatshape)
                    {
                        case 0: versh.Add(new circle(e.X, e.Y)); break;
                        case 1: versh.Add(new square(e.X, e.Y)); break;
                        case 2: versh.Add(new triangle(e.X, e.Y)); break;
                        default: versh.Add(new circle(e.X, e.Y)); break;
                    }
                Refresh();
            }
            Refresh();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            foreach (Draw v in versh)
            {
                v.DrawFigure(g);
            }
            if (NotNull)
            {
                
                if (versh.Count > 2)
                {
                    Pen pen = new Pen(Color.Black);
                    for (int i = 0; i < versh.Count; i++) versh[i].isInside = false;
                    for (int i = 0; i < versh.Count; i++)
                    {
                        for (int j = i + 1; j < versh.Count; j++)
                        {
                            bool Up = true;
                            bool Down = true;
                            for (int k = 0; k < versh.Count; k++)
                            {
                                if (k != i && k != j && i != j)
                                {
                                    if ((versh[i].SetY - versh[j].SetY) * versh[k].SetX + (versh[j].SetX - versh[i].SetX) * versh[k].SetY + (versh[i].SetX * versh[j].SetY - versh[j].SetX * versh[i].SetY) >= 0)
                                        Down = false;
                                    if ((versh[i].SetY - versh[j].SetY) * versh[k].SetX + (versh[j].SetX - versh[i].SetX) * versh[k].SetY + (versh[i].SetX * versh[j].SetY - versh[j].SetX * versh[i].SetY) < 0)
                                        Up = false;
                                }
                            }
                            if (Down == true || Up == true)
                            {
                                versh[i].isInside = true;
                                versh[j].isInside = true;
                                e.Graphics.DrawLine(pen, versh[i].SetX, versh[i].SetY, versh[j].SetX, versh[j].SetY);
                            }
                        }
                    }
                }
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            foreach(Draw v in versh)
            { 
            if(v.drag)
            {
                v.drag = false;
                Refresh();
            }
            }
            if (versh.Count > 2)
                for (int i = 0; i < versh.Count; i++)
                {
                    if (versh[i].isInside==false)
                        versh.Remove(versh[i]);
                }
            if (figureGrab)
            {
                for (int i = 0; i < versh.Count; i++)
                {
                    versh[i].drag = false;
                }
                figureGrab = false;
            }
            Refresh();
        }
    }
}
