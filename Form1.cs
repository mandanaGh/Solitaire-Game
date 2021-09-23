using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace CardGame_Project
{
   
    public partial class Form1 : Form
    {
        public MovablePicBox[] CardsArray = new MovablePicBox[52];
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            

            //*****************************************************************************************
            int i, j;
            int Counter = 0;

            for (i = 0; i < 52; i++)
            {
                CardsArray[Counter] = new MovablePicBox();
                switch (i % 4)
                {
                    case 0:
                        {
                            CardsArray[Counter].Suit = Convert.ToInt32(SuitCard.CLUBS);
                            CardsArray[Counter].Color = Convert.ToInt32(ColorCard.BLACK);
                            break;
                        }
                    case 1:
                        {
                            CardsArray[Counter].Suit = Convert.ToInt32(SuitCard.SPADES);
                            CardsArray[Counter].Color = Convert.ToInt32(ColorCard.BLACK);
                            break;
                        }
                    case 2:
                        {
                            CardsArray[Counter].Suit = Convert.ToInt32(SuitCard.HEARTS);
                            CardsArray[Counter].Color = Convert.ToInt32(ColorCard.RED);
                            break;
                        }
                    case 3:
                        {
                            CardsArray[Counter].Suit = Convert.ToInt32(SuitCard.DIAMOND);
                            CardsArray[Counter].Color = Convert.ToInt32(ColorCard.RED);
                            break;
                        }
                }

                CardsArray[Counter].Number = (i / 4) + 1;
                CardsArray[Counter].Img = Image.FromFile(@"..\..\Cards\" + (i + 1).ToString() + ".gif");

                Counter += 2;
                if (Counter >= 52)
                    Counter = 1;

            }

            //*****************************************************************************************

            int Count = 51;
            for (i = 1; i <= 7; i++)
            {
                for (j = 0; j < i ; j++)
                {
                    MovablePicBox.GameArray[i, j] = RandomObject(ref CardsArray, Count);

                    if (j != i-1)
                    {
                        MovablePicBox.GameArray[i, j].Image = Image.FromFile(@"..\..\Cards\back2.jpg");
                        MovablePicBox.GameArray[i, j].active = false;
                    }
                    else
                    {
                        MovablePicBox.GameArray[i, j].Image = MovablePicBox.GameArray[i, j].Img;
                        MovablePicBox.GameArray[i, j].active = true;

                    }

                    MovablePicBox.GameArray[i, j].Column = j;
                    MovablePicBox.GameArray[i, j].Status = 1;
                    MovablePicBox.GameArray[i, j].Left = MovablePicBox.GameArray[i, j].Initial_Left = Convert.ToInt32(LocationCard.Column_Left) + (i-1) * 125;
                    MovablePicBox.GameArray[i, j].Top = MovablePicBox.GameArray[i, j].Initial_Top = Convert.ToInt32(LocationCard.Column_Top) + j * 10;

                    Controls.Add(MovablePicBox.GameArray[i, j]);
                    this.Controls.SetChildIndex(MovablePicBox.GameArray[i, j], 0);
                    Count--;
                }
            }

            
            for (i = 0; i < 24; i++)
            {
                MovablePicBox.GameArray[0, i] = RandomObject(ref CardsArray, Count);

                MovablePicBox.GameArray[0, i].Image = Image.FromFile(@"..\..\Cards\back2.jpg");
                MovablePicBox.GameArray[0, i].active = false;

                MovablePicBox.GameArray[0, i].Column = i;
                MovablePicBox.GameArray[0, i].Status = 0;
                MovablePicBox.GameArray[0, i].Left = MovablePicBox.GameArray[0, i].Initial_Left = Convert.ToInt32(LocationCard.Column_Left);
                MovablePicBox.GameArray[0, i].Top = MovablePicBox.GameArray[0, i].Initial_Top = 35;

                Controls.Add(MovablePicBox.GameArray[0, i]);
                this.Controls.SetChildIndex(MovablePicBox.GameArray[0, i], 0);
                Count--;
            }
        }


        private MovablePicBox RandomObject(ref MovablePicBox[] CardsArray,int Count)
        {

            Random rm = new Random();
            
            int x = rm.Next(0, Count);
            MovablePicBox pic = CardsArray[x];
            CardsArray[x] = CardsArray[Count];
            return pic;
            
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo s = new ProcessStartInfo();
            Application.Exit();
            s.FileName = Assembly.GetExecutingAssembly().GetName().CodeBase;
            s.UseShellExecute = true;
            Process.Start(s);
        }
    }
}
