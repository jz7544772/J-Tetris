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
    public partial class Piece : PictureBox
    {
        int rowCount;
        int columnCount;

        int rotation;
        string name;
        Color color;
        int [, ] shape;
        Bitmap[] shapeImages;

        public Piece(string name, int rotation = 0)
        {
            this.name = name;
            this.rotation = rotation;
            FormPiece();

            InitializeComponent();
        }

        public void FormPiece()
        {
            switch(this.name)
            {
                case "stick": 
                    if(this.rotation == 0)
                    {
                        this.shape = new int[ , ] { 
                            { 1,1,1,1}
                        };
                        this.rowCount = 1;
                        this.columnCount = 4;
                    }
                    else if(this.rotation == 1)
                    {
                        shape = new int[ , ] { 
                            {1}, {1}, {1}, {1}
                        };
                        this.rowCount = 4;
                        this.columnCount = 1;
                    }
                    this.color = Color.Red;
                    this.shapeImages = new Bitmap[2] { null, null};
                    this.Location = new Point(0, 0);
                    this.Size = new Size(this.columnCount * 30, this.rowCount * 25);
                    break;
            }
            DrawShape();
        }

        public void DrawShape()
        {
            if(this.shapeImages[this.rotation] != null)
            {
                this.Image = this.shapeImages[this.rotation];
            }
            else
            {
                Bitmap shapeImage = new Bitmap(this.Width, this.Height);
                Pen shapePen = new Pen(Color.White, 1);

                SolidBrush shapeBrush = new SolidBrush(this.color);
                Rectangle tempRect;

                using (Graphics g = Graphics.FromImage(shapeImage))
                {
                    for (int row = 0; row < this.rowCount; row++)
                    {
                        for (int column = 0; column < this.columnCount; column++)
                        {
                            if (this.shape[row, column] == 1)
                            {
                                tempRect = new Rectangle(column * 30, row * 25, 30, 25);
                                g.FillRectangle(shapeBrush, tempRect);
                                g.DrawRectangle(shapePen, tempRect);
                            }
                        }
                    }
                }
                this.Image = shapeImage;
                this.shapeImages[this.rotation] = shapeImage;
            }
        }

        public void PerformRotation()
        {
            switch(this.name)
            {
                case "stick":
                    this.rotation = (this.rotation + 1) % 2;
                    FormPiece();
                    break;
            }
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
