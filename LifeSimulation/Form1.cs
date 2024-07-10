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

        private int life = 0;
        private int dead = 0;
        private int rLife= 0;
        private int rDead = 0;

        private bool[,] field2;//new
        private int rows2;
        private int cols2;

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
            Text= $"Поколения {currentGeneration}";

            nudScale.Enabled = false;
            nudDensite.Enabled = false;

            scale = (int)nudScale.Value;
            rows = pictureBox1.Height / scale;
            cols = pictureBox1.Width / scale;
            field = new bool[cols, rows];
            //red
            scale = (int)nudScale.Value;
            rows2 = pictureBox1.Height / scale;
            cols2 = pictureBox1.Width / scale;
            field2 = new bool[cols2, rows2];
            
           
            //green
            Random random = new Random();
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    if(random.Next((int)nudDensite.Value) == 0)
                    {
                        field[x, y] = true;
                        life++;
                    }
                    
                }
            }
            //red
            
            for (int x = 0; x < cols2; x++)
            {
                for (int y = 0; y < rows2; y++)
                {
                    if (random.Next((int)nudDensite.Value) == 0)
                    {
                        field2[x, y] = true;
                        rLife++;
                    }
                }
            }


            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            /////

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }
        
       
        //Проверка условия жизни создание или смерть
        private void NextGeneration()
        {
            graphics.Clear(Color.Black);
           
            var newField = new bool[cols, rows];
           //red
            var newField2= new bool[cols2, rows2];

            for (int x2 = 0; x2 < cols2; x2++)
            {
                for (int y2 = 0; y2 < rows2; y2++)
                {
                    var neighboursCount = Neighbours2(x2, y2);
                    var hasLife2 = field2[x2, y2];

                    if (!hasLife2 && neighboursCount == 2 && newField2[x2, y2] != true)
                    {

                        rLife++;
                        newField2[x2, y2] = true;

                    }
                    else if (hasLife2 && (neighboursCount < 1 || neighboursCount > 2))
                    {
                        newField2[x2, y2] = false;
                        rDead++;
                    }
                    else
                    {
                        newField2[x2, y2] = field2[x2, y2];

                    }
                    if (hasLife2)
                    {

                        graphics.FillRectangle(Brushes.Crimson, x2 * scale, y2 * scale, scale - 1, scale - 1);

                        labRedDead.Text = $"Умерло {rDead}";
                        labRedLife.Text = $"Появилось {rLife}";
                    }
                }
            }
            field2 = newField2;
            //green
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighboursCount = Neighbours(x, y);
                    var hasLife = field[x, y];
                    
                    if (!hasLife && neighboursCount == 3 && newField[x,y] !=true)
                    {
                        
                        life++;
                        newField[x, y] = true;
                        
                    }
                    else if (hasLife && (neighboursCount < 2 || neighboursCount > 3)) 
                    {
                        newField[x, y] = false;
                        dead++;
                    }
                    else
                    {
                        newField[x, y] = field[x, y];
                        
                    }
                    if (hasLife)
                    {
                        
                        graphics.FillRectangle(Brushes.GreenYellow, x * scale, y * scale, scale-1, scale-1);
                        
                        
                        
                        labDead.Text = $"Умерло {dead}";
                        labLife.Text = $"Появилось {life}";
                    }
                }
            }
          
            field = newField;
            pictureBox1.Refresh();

            Text = $"Поколения {++currentGeneration}";
        }
        
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
        ///new
        private int Neighbours2(int x, int y)
        {
            int count2 = 0;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var col2 = (x + i + cols2) % cols2;                //проверка на выходы за границу
                    var row2 = (y + j + rows2) % rows2;


                    var isChecking = col2 == x && row2 == y;
                    var hasLife2 = field2[col2, row2];


                    if (hasLife2 && !isChecking)
                        count2++;
                }
            }
            return count2;
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
                {
                    life++;
                    field2[x, y] = true;
                    
                    
                }

            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / scale;
                var y = e.Location.Y / scale;

                var validatePassed = ValidateMouse(x, y);
                if (validatePassed) 
                {
                    dead++;
                    field2[x, y] = false;
                    
                }
                   
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

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
