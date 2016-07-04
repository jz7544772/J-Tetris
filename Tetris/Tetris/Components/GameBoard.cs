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

        public GameBoard()
        {
            this.BackColor = Color.Orange;
            this.SetBounds(0, 0, 300, 550);
            this.Name = "GameBoard";

            DrawGrids();
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
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

        public void GeneratePiece()
        {
            currentPiece = new Piece("stick"); 
            this.Controls.Add(currentPiece);
        }

        public void RotatePiece()
        {
            currentPiece.PerformRotation();
        }

        public void LowerPiece() { }

        public void DropPiece() { }
    }
}
