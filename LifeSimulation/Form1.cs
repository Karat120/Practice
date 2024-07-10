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
        private int currentGeneration = 0;
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

            currentGeneration = 0;
            Text=$"Generation {currentGeneration}";

            nudScale.Enabled = false;
            nudDensite.Enabled = false;

            scale = (int)nudScale.Value;
            rows = pictureBox1.Height / scale;
            cols = pictureBox1.Width / scale;
            field = new bool[cols, rows];

            Random random = new Random();
            for (int x = 0; x < cols; x++) 
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next((int)nudDensite.Value) == 0; 
                }
            }


            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }
        private void NextGeneration()
        {
            graphics.Clear(Color.Black);

            var newField = new bool[cols, rows];
           
          
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighboursCount = Neighbours(x, y);
                    var hasLife = field[x, y];

                    if (!hasLife && neighboursCount == 3)
                    {
                        newField[x, y] = true;
                    }
                    else if (hasLife && (neighboursCount < 2 || neighboursCount > 3)) //данные по созданию новой клетки
                    {
                        newField[x, y] = false;
                    }
                    else
                    {
                        newField[x, y] = field[x, y];
                    }
                    if (hasLife)
                    {
                        graphics.FillRectangle(Brushes.GreenYellow, x * scale, y * scale, scale, scale);
                    }
                }
            }
            field = newField;
            pictureBox1.Refresh();

            Text = $"Generation {currentGeneration}";
        }
        //Проверка условия жизни создание или смерть
        private int Neighbours(int x,int y)
        {
            int count = 0;

            for (int i = -1;i<2;i++)
            {
                for(int j = -1;j<2;j++)
                {
                    var col = (x + i+cols)%cols;                //проверка на выходы за границу
                    var row = (y + j+rows)%rows;


                    var isChecking=col==x && row==y;
                    var hasLife = field[col,row];
                    
                    
                    if (hasLife && !isChecking)
                        count++;
                }
            } 
            return count;
        }
        private void StopGame()
        {
            if (!timer1.Enabled)
                return;
            timer1.Stop();
            nudScale.Enabled =true;
            nudDensite.Enabled = true;

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
           
            
                NextGeneration();
            
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(!timer1.Enabled)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / scale;
                var y = e.Location.Y / scale;

                var validatePassed=ValidateMouse(x, y);
                if (validatePassed)
                    field[x, y] = true;
            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / scale;
                var y = e.Location.Y / scale;

                var validatePassed = ValidateMouse(x, y);
                if (validatePassed)
                    field[x, y] = false;
            }
        }
        private bool ValidateMouse(int x, int y)
        {
            return x >=0 && y >= 0 && x<cols && y<rows;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = $"Generation {currentGeneration}";
        }
    }
}
