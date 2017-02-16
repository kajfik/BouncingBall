using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BouncingBall
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static Bitmap bmp;
        static Graphics g;
        static int w;
        static int h;
        static vector2D gravity;
        const int N = 20;
        static Ball[] balls;
        Random rnd = new Random();

        static double dist(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        struct vector2D
        {
            public double x;
            public double y;

            public void add(vector2D v)
            {
                this.x += v.x;
                this.y += v.y;
            }
        }

        class Ball
        {
            public vector2D pos;
            public int diameter;
            public double radius;
            public vector2D vel;

            public Ball(double x, double y, int diameter, vector2D vel)
            {
                this.pos.x = x;
                this.pos.y = y;
                this.diameter = diameter;
                this.radius = diameter / 2.0;
                this.vel = vel;
            }

            public void update(int index)
            {
                if (Math.Abs(this.vel.y) <= gravity.y && this.pos.y >= h - this.diameter - Math.Ceiling(gravity.y))
                {
                    this.vel.y = 0;
                    this.pos.y = h - this.diameter;
                }
                if (Math.Abs(this.vel.x) <= 0.15 && this.pos.y >= h - this.diameter)
                {
                    this.vel.x = 0;
                }
                pos.add(vel);

                if (this.pos.y >= h - this.diameter)
                {
                    if (this.vel.y != 0)
                    {
                        this.vel.y = -(this.vel.y / Math.Sqrt(2));
                    }

                    this.vel.x *= 0.985;
                    this.pos.y = h - this.diameter;
                }
                else if (this.pos.y < h - this.diameter)
                {
                    this.vel.add(gravity);
                }

                if (this.pos.y < 0)
                {
                    this.vel.y = -this.vel.y;
                    this.pos.y = 0;
                }

                if (this.pos.x > w - this.diameter)
                {
                    this.vel.x = -(this.vel.x / Math.Sqrt(2));
                    this.pos.x = w - this.diameter;
                }
                else if (this.pos.x < 0)
                {
                    this.vel.x = -(this.vel.x / Math.Sqrt(2));
                    this.pos.x = 0;
                }

                /*for (int i = 0; i < N; i++)
                {
                    if (i != index)
                    {
                        if(dist(this.pos.x + this.radius, this.pos.y + this.radius, balls[i].pos.x + balls[i].radius, balls[i].pos.y + balls[i].radius) < (this.radius + balls[i].radius))
                        {
                            this.vel.x *= -1;
                            balls[i].vel.x *= -1;
                        }
                    }
                }*/
            }

            public void show()
            {
                g.FillEllipse(Brushes.Black, Convert.ToInt32(this.pos.x), Convert.ToInt32(this.pos.y), diameter, diameter);
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            w = pictureBox1.Width;
            h = pictureBox1.Height;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            gravity.x = 0;
            gravity.y = 0.2;
            vector2D vel = new vector2D();
            

            balls = new Ball[N];
            for (int i = 0; i < N; i++)
            {
                int diam = rnd.Next(10, 40);
                vel.x = rnd.Next(-10, 10);
                vel.y = rnd.Next(-10, 10);
                balls[i] = new Ball(rnd.Next(w), rnd.Next(h) - diam, diam, vel);
                balls[i].show();
            };
            pictureBox1.Image = bmp;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            g.DrawRectangle(Pens.Black, 0, 0, w - 1, h - 1);
            for (int i = 0; i < N; i++)
            {
                balls[i].update(i);
                balls[i].show();
            }
            
            pictureBox1.Image = bmp;
            pictureBox1.Update();
            pictureBox1.Show();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < N; i++)
            {
                balls[i].vel.x += ((balls[i].pos.x + balls[i].radius) - e.X) / 20.0;
                //balls[i].vel.y += ((balls[i].pos.y + balls[i].radius) - e.Y) / 20.0;
                /*if (e.X < balls[i].pos.x + balls[i].radius)
                {
                    balls[i].vel.x += 10;
                }
                else
                {
                    balls[i].vel.x -= 10;
                }*/
                if (e.Y < balls[i].pos.y + balls[i].radius)
                {
                    balls[i].vel.y += 10 - Math.Abs((balls[i].pos.x + balls[i].radius) - e.X) / 20.0;
                }
                else
                {
                    balls[i].vel.y -= 10 - Math.Abs((balls[i].pos.x + balls[i].radius) - e.X) / 20.0;
                }
            }

        }
    }
}
