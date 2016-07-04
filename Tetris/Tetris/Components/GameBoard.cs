using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tetris.Components
{
    public partial class GameBoard : PictureBox
    {
        int[,] cells = new int[22, 10];
        int columnCount = 10;
        int rowCount = 22;
        int cellWidth = 30;
        int cellHeight = 25;

        enum PieceNames { stick = 1, square, tee, ess, zed, jay, el };

        Piece currentPiece;

        Timer gameTimer;

        Boolean gameOn;

        public GameBoard()
        {
            this.BackColor = Color.Orange;
            this.SetBounds(0, 0, 300, 550);
            this.Name = "GameBoard";

            gameTimer = new Timer();
            gameTimer.Interval = 500;
            gameTimer.Tick += new EventHandler(GameTimerTick);
            gameTimer.Start();

            DrawGrids();
            InitializeComponent();
        }

        private void GameTimerTick(object sender, EventArgs e)
        {
            if(gameOn)
            {
                if(currentPiece == null)
                {
                    GeneratePiece("stick");
                }
                LowerPiece();
            }
        }

        public void GameOnOff()
        {
            this.gameOn = !(this.gameOn);
        }

        private void DrawGrids()
        {
            Bitmap gridImage = new Bitmap(300, 550);
            Pen gridPen = new Pen(Color.White, 1);
            using (Graphics g = Graphics.FromImage(gridImage))
            {
                for(int column = 0; column < columnCount; column++)
                {
                    g.DrawLine(gridPen, column * cellWidth , 0, column* cellWidth, 550);
                }
                for (int row = 0; row < rowCount; row++)
                {
                    g.DrawLine(gridPen, 0, row*cellHeight, 300, row*cellHeight);
                }
            }
            this.Image = gridImage;
        }

        /*
        public void GeneratePiece()
        {
            currentPiece = new Piece("stick"); 
            this.Controls.Add(currentPiece);
        }
        */

        public void GeneratePiece(string pieceName)
        {
            currentPiece = new Piece(pieceName);
            this.Controls.Add(currentPiece);
        }

        public void RotatePiece()
        {
            if(currentPiece != null)
            {
                currentPiece.PerformRotation();
            }
        }

        // A helper function checking whether the currentPiece has landed
        public Boolean CollisionBottom() 
        {   
            if(this.currentPiece.Location.Y + this.currentPiece.Height >= this.Height)
            {
                return true;
            }
            return false;
        }
        public Boolean CollisionLeft()
        {
            if(this.currentPiece.Location.X <= 0)
            {
                return true;
            }
            return false;
        }
        public Boolean CollisionRight()
        {
            if (this.currentPiece.Location.X + this.currentPiece.Width >= this.Width)
            {
                return true;
            }
            return false;
        }

        public void LowerPiece() {
            if(!CollisionBottom())
            {
                this.currentPiece.Top += 25;
            }
        }
        public void RighterPiece()
        {
            if (!CollisionRight())
            {
                this.currentPiece.Left += 30;
            }
        }
        public void LefterPiece()
        {
            if (!CollisionLeft())
            {
                this.currentPiece.Left -= 30;
            }
        }

        public void LandPiece()
        {

        }

        public void DropPiece() { }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
