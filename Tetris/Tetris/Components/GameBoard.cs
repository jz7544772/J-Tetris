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
        int gridWidth;
        int columnCount;
        int gridHeight;
        int rowCount;
        int[][] grids;

        Dictionary<int, Color> PieceColors = new Dictionary<int, Color>();
        string[] PieceNames = new string[] { "stick", "square", "tee", "ess", "zed", "jay", "el"};
        Random rnd = new Random();

        Piece currentPiece;
        Timer gameTimer;

        Boolean gameOn;

        public GameBoard()
        {
            Initialize();

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
            PieceColors[8] = Color.Black; //boundary

            DrawGrids();
            InitializeComponent();
        }

        private void Initialize()
        {
            this.gridWidth = Properties.Settings.Default.gridWidth;
            this.columnCount = Properties.Settings.Default.columnCount;
            this.gridHeight = Properties.Settings.Default.gridHeight;
            this.rowCount = Properties.Settings.Default.rowCount;

            this.Size = new Size(
                this.gridWidth * this.columnCount,
                this.gridHeight * this.rowCount);

            this.Name = "GameBoard";

            this.grids = new int[this.rowCount][];
            for (int row = 0; row < this.rowCount -1 ; row++)
            {
                this.grids[row] = InitializeIntGridBoundarLeftRight(this.columnCount, 8);
            }
            this.grids[this.rowCount - 1] = InitializeIntGridBoundaryBottom(this.columnCount, 8);
        }

        private int[] InitializeIntGridBoundarLeftRight(int valueCount, int value)
        {
            int[] arry = new int[valueCount];
            arry[0] = 8;
            arry[valueCount - 1] = 8;
            return arry;
        }
        private int[] InitializeIntGridBoundaryBottom(int valueCount, int value)
        {
            int[] arry = new int[valueCount];
            for(int i=0; i < valueCount; i++)
            {
                arry[i] = value;
            }
            return arry;
        }

        private void GameTimerTick(object sender, EventArgs e)
        {
            if (gameOn)
            {
                if (currentPiece != null)
                {
                    LowerPiece();
                }
                else
                {
                    this.GeneratePiece(this.PieceNames[rnd.Next(0, 6)]);
                }
            }
        }

        public void GameOnOff()
        {
            this.gameOn = !(this.gameOn);
        }

        private void DrawGrids()
        {
            Bitmap gridImage = new Bitmap(this.Width, this.Height);
            Pen gridPen = new Pen(Color.Black, 1);

            Rectangle tempRect;
            SolidBrush gridBrush;

            int pieceCode;

            using (Graphics g = Graphics.FromImage(gridImage))
            {
                for (int row = 0; row < this.rowCount; row++)
                {
                    for (int column = 0; column < this.columnCount; column++)
                    {
                        tempRect = new Rectangle(
                            column * this.gridWidth, row * this.gridHeight,
                            this.gridWidth, this.gridHeight);

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
            }
            catch (Exception ex)
            {
                this.gameTimer.Stop();
                MessageBox.Show(
                    "GameBoard.GeneratePiece: " + "\n" +
                    ex.Message + "\n" +
                    ex.StackTrace
                );
            }
        }

        private Boolean CollisionPiece()
        {
            try
            {
                Boolean stop = false; 

                int currentPieceX = this.currentPiece.Location.X;
                int currentPieceY = this.currentPiece.Location.Y;

                int[,] nextRotation = this.currentPiece.GetNextRotation();
                int pieceRowCount = nextRotation.GetLength(0);
                int pieceColumnCount = nextRotation.GetLength(1);

                for(int row = 0; row < pieceRowCount; row++)
                {
                    for(int column = 0; column < pieceColumnCount; column+= pieceColumnCount - 1)
                    {
                        if (nextRotation[row, column] == 1)
                        {
                            int coordX = (currentPieceX + (column * this.gridWidth)) / this.gridWidth;
                            int coordY = (currentPieceY + (row * this.gridHeight)) / this.gridHeight;
                            if(this.grids[coordY][coordX] >0)
                            {
                                return true;
                            }

                            if (column == 0) // Leftmost
                            {
                                if (coordX <= 0)
                                {
                                    this.currentPiece.Left = this.currentPiece.Left + this.gridWidth * (0 - coordX + 1);
                                    return false;
                                }
                            }
                            else // Rightmost
                            {
                                
                                if(coordX >= this.columnCount - 1)
                                {
                                    this.currentPiece.Left = this.currentPiece.Left - this.gridWidth * (coordX - this.columnCount + 2);
                                }
                            }
                        }
                    }
                }
            } catch(Exception ex)
            {
                this.gameTimer.Stop();
                MessageBox.Show(
                    "GameBoard.CollisionPiece: " + "\n" +
                    ex.Message + "\n" +
                    ex.StackTrace
                );
            }

            return false;
        }

        public void RotatePiece()
        {
            if (!CollisionPiece())
            {
                currentPiece.Rotate();
            }
        }

        // A helper function checking whether the currentPiece has landed 
        // 1. landed on the bottom of the GameBoard
        // 2. landed on the top of another piece
        public Boolean CollisionBottom()
        {
            try
            {
                int currentPieceX = this.currentPiece.Location.X;
                int currentPieceY = this.currentPiece.Location.Y;

                int[,] pieceShape = this.currentPiece.GetRotation();
                int pieceRowCount = pieceShape.GetLength(0);
                int pieceColumnCount = pieceShape.GetLength(1);

                for (int row = pieceRowCount - 1; row >= 0; row--)
                {
                    for (int column = pieceColumnCount - 1; column >= 0; column--)
                    {
                        if (pieceShape[row, column] == 1)
                        {
                            int coordX = (currentPieceX + (column * this.gridWidth)) / this.gridWidth;
                            int coordY = (currentPieceY + (row * this.gridHeight)) / this.gridHeight;

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
            catch (Exception ex)
            {
                this.gameTimer.Stop();
                MessageBox.Show(
                    "GameBoard.CollisionBottom: " + "\n" +
                    ex.Message + "\n" +
                    ex.StackTrace
                );
            }

            return false;
        }
        public Boolean CollisionLeft()
        {
            try
            {
                int currentPieceX = this.currentPiece.Location.X;
                int currentPieceY = this.currentPiece.Location.Y;

                int[,] pieceShape = this.currentPiece.GetRotation();
                int pieceRowCount = pieceShape.GetLength(0);
                int pieceColumnCount = pieceShape.GetLength(1);

                for(int column = 0; column < pieceColumnCount; column++)
                {
                    for(int row = 0; row < pieceRowCount; row ++)
                    {
                        if(pieceShape[row, column] == 1)
                        {
                            int coordX = (currentPieceX + (column * this.gridWidth)) / this.gridWidth;
                            int coordY = (currentPieceY + (row * this.gridHeight)) / this.gridHeight;

                            if (this.grids[coordY][coordX - 1] != 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.gameTimer.Stop();
                MessageBox.Show(
                    "GameBoard.CollisionLeft: " + "\n" +
                    ex.Message + "\n" +
                    ex.StackTrace
                );
            }
            return false;
        }
        public Boolean CollisionRight()
        {
            try
            {
                int currentPieceX = this.currentPiece.Location.X;
                int currentPieceY = this.currentPiece.Location.Y;

                int[,] pieceShape = this.currentPiece.GetRotation();
                int pieceRowCount = pieceShape.GetLength(0);
                int pieceColumnCount = pieceShape.GetLength(1);

                for (int column = pieceColumnCount - 1; column >= 0; column--)
                {
                    for (int row = 0; row < pieceRowCount; row++)
                    {
                        if (pieceShape[row, column] == 1)
                        {
                            int coordX = (currentPieceX + (column * this.gridWidth)) / this.gridWidth;
                            int coordY = (currentPieceY + (row * this.gridHeight)) / this.gridHeight;

                            if (this.grids[coordY][coordX + 1] != 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.gameTimer.Stop();
                MessageBox.Show(
                    "GameBoard.CollisionRight: " + "\n" +
                    ex.Message + "\n" +
                    ex.StackTrace
                );
            }
            return false;
        }

        public void LowerPiece()
        {
            if (!CollisionBottom())
            {
                this.currentPiece.Top += this.gridHeight;
            }
        }
        public void RighterPiece()
        {
            if (!CollisionRight())
            {
                this.currentPiece.Left += this.gridWidth;
            }
        }
        public void LefterPiece()
        {
            if (!CollisionLeft())
            {
                this.currentPiece.Left -= this.gridWidth;
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
                        int coordX = (pieceX + (column * this.gridWidth)) / this.gridWidth;
                        int coordY = (pieceY + (row * this.gridHeight)) / this.gridHeight;
                        this.grids[coordY][coordX] = this.currentPiece.GetCode();
                    }
                }
            }

            CheckClear();
            this.Controls.Remove(this.currentPiece);
            this.currentPiece = null;
            DrawGrids();
        }

        private void CheckClear()
        {
            Boolean stop;
            for (int row = this.rowCount - 2; row >= 0; row--)
            {
                stop = false;
                for (int column = 0; column < this.columnCount && stop == false; column++)
                {
                    if (this.grids[row][column] == 0)
                    {
                        stop = true;
                    }
                }

                if (stop == false)
                {
                    for (int shiftRow = row; shiftRow >= 0; shiftRow--)
                    {
                        if (shiftRow == 0)
                        {
                            this.grids[shiftRow] = InitializeIntGridBoundarLeftRight(this.columnCount, 8);
                        }
                        else
                        {
                            this.grids[shiftRow] = this.grids[shiftRow - 1];
                        }
                    }

                    row = this.rowCount - 1;
                }
            }
            // TODO: If row is the first row, then the game ends
        }

        public void PrintGrids()
        {
            string s = "";
            for (int i = 0; i < this.rowCount; i++)
            {
                for (int j = 0; j < this.columnCount; j++)
                {
                    s += string.Format("{0} ", this.grids[i][j]);
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
