using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace CardGame_Project
{
    public partial class MovablePicBox : PictureBox
    {
       
        Point mdLoc;
        MovablePicBox[] MoveObject = new MovablePicBox[13];
        
        
        private int _Initial_Left;
        private int _Initial_Top;
        private int _Number;
        private int _Suit;
        private int _Status;
        private int _Color;
        private int _Column;
        private Image _Img;

        public bool active;
        public static MovablePicBox[,] GameArray = new MovablePicBox[12, 24];


        //******************************************************************************************************

        public int Initial_Left
        {
            get { return _Initial_Left; }
            set { _Initial_Left = value; }
        }

        public int Initial_Top
        {
            get { return _Initial_Top; }
            set { _Initial_Top = value; }
        }

        public int Number
        {
            get { return _Number; }
            set { _Number = value; }
        }

        public int Suit
        {
            get { return _Suit; }
            set { _Suit = value; }
        }

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public int Color
        {
            get { return _Color; }
            set { _Color = value; }
        }

        public int Column
        {
            get { return _Column; }
            set { _Column = value; }
        }

        public Image Img
        {
            get { return _Img; }
            set { _Img = value; }
        }

        //******************************************************************************************************
       

        public MovablePicBox()
        {
            this.Width = 72;
            this.Height = 96;
        }


        //******************************************************************************************************


        protected override void OnMouseDown(MouseEventArgs e)
        {

            mdLoc = e.Location;
            int count = 0;
            int i, j, x, y;

            if (this.active)
            {
                switch (this.Status)
                {
                    case 0:
                        {
                            MoveObject[0] = this;
                            Parent.Controls.SetChildIndex(this, 0);
                        }
                        break;
                    case 1:
                        {
                            x = (this.Initial_Left / 125) + 1;
                            y = this.Column;

                            
                            for (i = y; i < 24; i++)
                            {
                                if (GameArray[x, i] != null)
                                {
                                    MoveObject[count] = GameArray[x, i];
                                    count++;
                                }
                                else
                                    break;
                            }
                            
                            if (count > 1)
                                for (i = 0; i < count; i++)
                                {
                                    Parent.Controls.SetChildIndex(MoveObject[i], 0);
                                }
                            else
                                Parent.Controls.SetChildIndex(this, 0);

                        }
                        break;
                    case 2:
                        {
                            MoveObject[0] = this;
                            Parent.Controls.SetChildIndex(this, 0);
                        }
                        break;
                }
            }


        }

        //******************************************************************************************************

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (active)
            {
                if (e.Button == MouseButtons.Left)
                {
                    foreach (MovablePicBox item in MoveObject)
                    {
                        if (item != null)
                        {
                            item.Left += e.X - mdLoc.X;
                            item.Top += e.Y - mdLoc.Y;
                        }
                        else
                            break;
                    }
                }
            }

        }

        //******************************************************************************************************


        protected override void OnMouseUp(MouseEventArgs e)
        {

            int i, j;
            int x=0, y=0;
            int Col=0;
            int FinalStatus=0;
            bool cheak = false;
            bool verify = false;


            for (i = 1; i <= 7; i++)
            {
                x = Convert.ToInt32(LocationCard.Column_Left) + (i - 1) * 125;
                y = Convert.ToInt32(LocationCard.Column_Top);
                Col = 0;

                for (j = 0; j < 24; j++)
                    if (GameArray[i, j] == null)
                    {
                        if (j != 0)
                        {
                            y = GameArray[i, j - 1].Top;
                            Col = j;
                        }
                        break;
                    }

                if (((x < this.Left && this.Left < x + 72) || (x < this.Right && this.Right < x + 72)) && ((y < this.Top && this.Top < y + 96) || (y < this.Bottom && this.Bottom < y + 96)))
                {
                    FinalStatus = 1;
                    cheak = true;
                    break;
                }

            }

            if (!cheak)
            {
                for (i = 4; i <= 7; i++)
                {
                    x = Convert.ToInt32(LocationCard.Column_Left) + (i - 1) * 125;
                    y = 35;
                    Col = 0;

                    for (j = 0; j < 24; j++)
                        if (GameArray[i + 4, j] == null)
                        {
                            Col = j;
                            break;
                        }

                    if (((x < this.Left && this.Left < x + 72) || (x < this.Right && this.Right < x + 72)) && ((y < this.Top && this.Top < y + 96) || (y < this.Bottom && this.Bottom < y + 96)))
                    {
                        FinalStatus = 2;
                        cheak = true;
                        break;
                    }
                }
            }


            if (cheak)
            {
                verify = false;

                switch (FinalStatus)
                {
                    case 1:
                        {
                            i = (x / 125) + 1;
                            if (Col != 0)
                            {
                                if ((this.Number + 1 == GameArray[i, Col - 1].Number) && (this.Color != GameArray[i, Col - 1].Color) && GameArray[i, Col - 1].active == true)
                                    verify = true;
                            }
                            else
                                if (this.Number == 13)
                                    verify = true;


                            if (verify)
                            {
                                MoveFunction(ref MoveObject, FinalStatus, i, Col);
                                foreach (MovablePicBox item in MoveObject)
                                {
                                    if (item != null)
                                    {
                                        if (Col == 0)
                                        {
                                            if (item != MoveObject[0])
                                                y += 16;
                                        }
                                        else
                                            y += 16;
                                        
                                        item.Left = item.Initial_Left = x;
                                        item.Top = item.Initial_Top = y;

                                    }
                                    else
                                        break;
                                }
                            }

                        }
                        break;
                    case 2:
                        {
                            i = (x / 125) + 5;
                            if (Col != 0)
                            {
                                if ((this.Number - 1 == GameArray[i, Col - 1].Number) && (this.Suit == GameArray[i, Col - 1].Suit))
                                    verify = true;
                            }
                            else
                                if (this.Number == 1)
                                    verify = true;

                            if (verify)
                            {
                                MoveFunction(ref MoveObject, FinalStatus, i, Col);
                                foreach (MovablePicBox item in MoveObject)
                                {
                                    if (item != null)
                                    {
                                        item.Left = item.Initial_Left = x;
                                        item.Top = item.Initial_Top = y;

                                    }
                                    else
                                        break;
                                }
                            }
                        }
                        break;
                }

            }
           
            if ((!cheak) || ((cheak) && (!verify)))
            {
                foreach (MovablePicBox item in MoveObject)
                {
                    if (item != null)
                    {
                        item.Left = item.Initial_Left;
                        item.Top = item.Initial_Top;
                    }
                    else
                        break;
                }
            }


            for (i = 0; i < 13; i++)
                MoveObject[i] = null;


        }

        

        //******************************************************************************************************


        protected override void OnClick(EventArgs e)
        {
            if (Number == 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (GameArray[0,i] != null)
                    {
                        GameArray[0, i].Image = Image.FromFile(@"..\..\Cards\back2.jpg");
                        GameArray[0, i].active = false;
                        GameArray[0, i].Left = MovablePicBox.GameArray[0, i].Initial_Left = Convert.ToInt32(LocationCard.Column_Left);
                        GameArray[0, i].Top = MovablePicBox.GameArray[0, i].Initial_Top = 35;
                        Parent.Controls.SetChildIndex(GameArray[0, i], 0);

                    }
                }
            }
            else
            {
                bool cheak = false;
                if (!this.active)
                {

                    if (this.Status == 0)
                    {
                        this.Left = this.Initial_Left = Convert.ToInt32(LocationCard.Column_Left) + 125;
                        cheak = true;
                    }
                    else
                    {

                        int x = (this.Left / 125) + 1;
                        int y = this.Column;
                        if (GameArray[x, y + 1] == null)
                        {
                            cheak = true;
                        }
                    }

                    if (cheak)
                    {
                        this.active = true;
                        this.Image = this.Img;
                        this.Refresh();
                        Parent.Controls.SetChildIndex(this, 0);
                    }
                }
            }
        }


        //******************************************************************************************************


        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            int i,j,x,y;
            int FinalRow=0, FinalCol=0;
            bool cheak=false;


            if (this.active)
            {

                if (this.Number == 1)
                {
                    for(i=8 ; i<12 ; i++)
                        if (GameArray[i, 0] == null)
                        {
                            FinalRow = i;
                            FinalCol = 0;
                            cheak=true;
                            break;
                        }
                }
                else
                {
                    for (i = 8; i < 12; i++)
                        if (GameArray[i,0] != null)
                        {
                            if (this.Suit == GameArray[i, 0].Suit)
                            {
                                for (j = 0; j < 13; j++)
                                {
                                    if (GameArray[i, j] == null)
                                        if (this.Number - 1 == GameArray[i, j - 1].Number)
                                        {
                                            FinalRow = i;
                                            FinalCol = j;
                                            cheak = true;
                                            break;
                                        }
                                        else
                                            break;

                                }
                                break;
                            }
                        }
                }


                MoveObject[0] = this;
                Parent.Controls.SetChildIndex(this, 0);

                if (cheak)
                {
                    MoveFunction(ref MoveObject, 2, FinalRow, FinalCol);

                    x = Convert.ToInt32(LocationCard.Column_Left) + (FinalRow - 5) * 125;
                    y = 35;

                    MoveObject[0].Left = MoveObject[0].Initial_Left = x;
                    MoveObject[0].Top = MoveObject[0].Initial_Top = y;

                    MoveObject[0] = null;
                }
            }
        }

        //******************************************************************************************************


        private void MoveFunction(ref MovablePicBox[] MoveObject, int FinalStatus,int FinalRow, int FinalColumn)
        {

            int i, j,row=0,col;
            int FirstStatus = MoveObject[0].Status;


            switch (FirstStatus)
            {
                case 0:
                    {
                        row = 0;
                    }
                    break;
                case 1:
                    {
                        row = (MoveObject[0].Initial_Left / 125) + 1;
                    }
                    break;
                case 2:
                    {
                        row = (MoveObject[0].Initial_Left / 125) + 5;
                    }
                    break;
            }

            col = MoveObject[0].Column;
            if (FirstStatus == 1)
            {

                for (j = col; j < 24; j++)
                    if (GameArray[row, j] != null)
                        GameArray[row, j] = null;
                    else
                        break;
            }
            else
                GameArray[row, col] = null;


            int count = FinalColumn;
            for (i = 0; i < 13; i++)
                if (MoveObject[i] != null)
                {

                    GameArray[FinalRow, count] = MoveObject[i];
                    GameArray[FinalRow, count].Column = count;
                    GameArray[FinalRow, count].Status = FinalStatus;
                    count++;
                }
                else
                    break;


            if (FinalStatus==2)
                if (GameArray[8, 12] != null && GameArray[9, 12] != null && GameArray[10, 12] != null && GameArray[11, 12] != null)
                {
                    if ((new Form2()).ShowDialog() == DialogResult.No)
                    {
                        this.Parent.FindForm().Close();
                    }
                    else
                    {
                        ProcessStartInfo s = new ProcessStartInfo();
                        Application.Exit();
                        s.FileName = Assembly.GetExecutingAssembly().GetName().CodeBase;
                        s.UseShellExecute = true;
                        Process.Start(s);
                    }
                }
        }
    }
}
