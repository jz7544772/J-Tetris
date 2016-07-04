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

        private void mainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Space:
                    this.GameBoard.GeneratePiece();
                    break;
                case Keys.Up:
                    this.GameBoard.RotatePiece();
                    break;
            }
        }
    }


}
