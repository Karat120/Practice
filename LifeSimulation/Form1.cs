using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeSimulation
{
    public partial class Form1 : Form
    {

        private Graphics graphics;
        private int scale;
        private bool[,] field; //жизнь/смерть
        private int rows;
        private int cols;
        public Form1()
        {
            InitializeComponent();
           
        }
        private void StartGame()
        {
            if (timer1.Enabled) 
            {
                return;
            }

            nudScale.Enabled = false;
            nudDensite.Enabled = false;

            scale = (int)nudScale.Value;
            rows = pictureBox1.Height / scale;
            cols = pictureBox1.Width / scale;
            field = new bool[cols, rows];

            Random random = new Random();
            for (int i = 0; i < cols; i++) 
            {
                for (int j = 0; j < rows; j++)
                {
                    field[i, j] = random.Next((int)nudDensite.Value) == 0; 
                }
            }


            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void bStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void bStop_Click(object sender, EventArgs e)
        {

        }
    }
}
