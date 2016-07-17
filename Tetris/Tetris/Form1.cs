using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tetris
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Up:
                    GameBoard.RotatePiece();
                    break;
                case Keys.Left:
                    GameBoard.LefterPiece();
                    break;
                case Keys.Right:
                    GameBoard.RighterPiece();
                    break;
                case Keys.Down:
                    GameBoard.LowerPiece();
                    break;
                case Keys.Enter:
                    GameBoard.GameOnOff();
                    break;
                case Keys.Space:
                    GameBoard.GeneratePiece("stick");
                    break;
                case Keys.S:
                    GameBoard.GeneratePiece("square");
                    break;
                case Keys.T:
                    GameBoard.GeneratePiece("tee");
                    break;
                case Keys.E:
                    GameBoard.GeneratePiece("ess");
                    break;
                case Keys.Z:
                    GameBoard.GeneratePiece("zed");
                    break;
                case Keys.J:
                    GameBoard.GeneratePiece("jay");
                    break;
                case Keys.L:
                    GameBoard.GeneratePiece("el");
                    break;
                case Keys.M:
                    GameBoard.PrintGrids();
                    break;
            }

        }
    }


}
