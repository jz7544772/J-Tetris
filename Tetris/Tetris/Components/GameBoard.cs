using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Tetris.Components
{
    public partial class GameBoard : PictureBox
    {
        int[][] grids = new int[22][]; 
        int columnCount = 10;
        int rowCount = 22;

        Dictionary<int, Color> PieceColors = new Dictionary<int, Color>();
        Piece currentPiece;
        Timer gameTimer;

        Boolean gameOn;

        public GameBoard()
        {
            for (int row = 0; row < rowCount; row++) {
                this.grids[row] = new int[columnCount];
            }

            this.SetBounds(0, 0, 300, 550);
            this.Name = "GameBoard";

            gameTimer = new Timer();
            gameTimer.Interval = 500;
            gameTimer.Tick += new EventHandler(GameTimerTick);
            gameTimer.Start();

            PieceColors[0] = Color.White;  //empty
            PieceColors[1] = Color.Cyan; //stick
            PieceColors[2] = Color.LightGray; // square
            PieceColors[3] = Color.Yellow; // tee
            PieceColors[4] = Color.Lime; // ess
            PieceColors[5] = Color.Red; // zed
            PieceColors[6] = Color.Purple; //jay
            PieceColors[7] = Color.Magenta; //el

            DrawGrids();
            InitializeComponent();
        }

        private void GameTimerTick(object sender, EventArgs e)
        {
            if (gameOn)
            {
                if (currentPiece != null)
                {
                    LowerPiece();
                }
                
            }
        }

        public void GameOnOff()
        {
            this.gameOn = !(this.gameOn);
        }

        private void DrawGrids()
        {
            Bitmap gridImage = new Bitmap(300, 550);
            Pen gridPen = new Pen(Color.Black, 1);

            Rectangle tempRect;
            SolidBrush gridBrush;

            int pieceCode;

            using (Graphics g = Graphics.FromImage(gridImage))
            {
                for (int row = 0; row < rowCount; row++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        tempRect = new Rectangle(column * 30, row * 25, 30, 25);

                        pieceCode = this.grids[row][column];
                        gridBrush = new SolidBrush(this.PieceColors[pieceCode]);
                        g.FillRectangle(gridBrush, tempRect);
                        g.DrawRectangle(gridPen, tempRect);
                    }
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
            try
            {
                currentPiece = new Piece(pieceName);
                this.Controls.Add(currentPiece);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                this.gameTimer.Stop();
            }
        }

        public void RotatePiece()
        {
            if (currentPiece != null)
            {
                currentPiece.PerformRotation();
            }
        }

        // A helper function checking whether the currentPiece has landed 
        // 1. landed on the bottom of the GameBoard
        // 2. landed on the top of another piece
        public Boolean CollisionBottom()
        {
            try {
                int currentPieceX = this.currentPiece.Location.X;
                int currentPieceY = this.currentPiece.Location.Y;

                int[,] pieceShape = this.currentPiece.GetShape();
                int pieceRowCount = pieceShape.GetLength(0);
                int pieceColumnCount = pieceShape.GetLength(1);

                if (currentPieceY + this.currentPiece.Height >= this.Height)
                {
                    this.LandPiece(pieceShape, currentPieceX, currentPieceY);
                    return true;
                }
                else
                {
                    for (int row = pieceRowCount - 1; row >= 0; row--)
                    {
                        for (int column = pieceColumnCount - 1; column >= 0; column--)
                        {
                            if (pieceShape[row, column] == 1)
                            {
                                int coordX = (currentPieceX + (column * 30)) / 30;
                                int coordY = (currentPieceY + (row * 25)) / 25;

                                if (coordY <= rowCount - 2) { coordY += 1; };

                                if (this.grids[coordY][coordX] != 0)
                                {
                                    this.LandPiece(pieceShape, currentPieceX, currentPieceY, 1);
                                    return true;
                                }
                            }
                        }
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return false;
        }
        public Boolean CollisionLeft()
        {
            if (this.currentPiece.Location.X <= 0)
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
            if (!CollisionBottom())
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

        public void LandPiece(int[,] pieceShape, int pieceX, int pieceY, int lookAhead = 0)
        {
            int pieceRowCount = pieceShape.GetLength(0);
            int pieceColumnCount = pieceShape.GetLength(1);

            for (int row = pieceRowCount - 1; row >= 0; row--)
            {
                for (int column = pieceColumnCount - 1; column >= 0; column--)
                {

                    if (pieceShape[row, column] == 1)
                    {
                        int coordX = (pieceX + (column * 30)) / 30;
                        int coordY = (pieceY + (row * 25)) / 25;


                        this.grids[coordY][coordX] = this.currentPiece.GetCode();
                    }
                }
            }

            CheckClear();
            this.Controls.Remove(this.currentPiece);
            this.currentPiece = null;
            DrawGrids();
        }

        private void CheckClear () {
            Boolean stop;
            for (int row = rowCount - 1; row >= 0; row--)
            {
                stop = false;
                for (int column = 0; column < columnCount && stop == false; column++)
                {
                    if(this.grids[row][column] == 0)
                    {
                        stop = true;
                    }
                }

                if(stop == false)
                {
                    for (int shiftRow = row; shiftRow >= 0; shiftRow--)
                    {
                        if (shiftRow == 0)
                        {
                            this.grids[shiftRow] = new int[columnCount];
                        }
                        else
                        {
                            this.grids[shiftRow] = this.grids[shiftRow - 1];
                        }
                    }

                    row = rowCount - 1;
                }
            }
               

            int[] tempRow;
            // TODO: If row is the first row, then the game ends
            
        }

        public void PrintGrids()
        {
            string s = "";
            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                   s+=string.Format("{0} ", this.grids[i][ j]);
                }
                s += Environment.NewLine;
            }

            MessageBox.Show(s);
        }

        public void DropPiece() { }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
