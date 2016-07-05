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
        int[, ] shape;
        int[][,] shapes;
        Bitmap[] shapeImages;
        int[,] shapeSizes;
        int code;

        Boolean rotated;

        public Piece(string name, int rotation = 0)
        {
            this.name = name;
            this.rotation = rotation;
            this.Location = new Point(0, 0);
            FormPiece();

            InitializeComponent();
        }

        public void FormPiece()
        {
            switch (this.name)
            {
                case "stick":
                    if (this.rotation == 0)
                    {
                        this.shape = new int[,]
                        {
                            { 1,1,1,1}
                        };
                        this.rowCount = 1;
                        this.columnCount = 4;
                    }
                    else if (this.rotation == 1)
                    {
                        this.shape = new int[,]
                        {
                            {1},
                            {1},
                            {1},
                            {1}
                        };
                        this.rowCount = 4;
                        this.columnCount = 1;
                    }

                    if (!this.rotated)
                    {
                        this.code = 1;
                        this.color = Color.Cyan;
                        this.shapeImages = new Bitmap[2] { null, null };
                        this.shapeSizes = new int[2, 2];
                    }
                    break;

                case "tee":
                    if (this.rotation == 0)
                    {
                        this.shape = new int[,]
                        {
                            { 1, 1, 1 },
                            { 0, 1, 0 }
                        };
                        this.rowCount = 2;
                        this.columnCount = 3;
                    }
                    else if (this.rotation == 1)
                    {
                        this.shape = new int[,]
                        {
                            { 0, 1 },
                            { 1, 1 },
                            { 0, 1 }
                        };
                        this.rowCount = 3;
                        this.columnCount = 2;
                    }
                    else if (this.rotation == 2)
                    {
                        this.shape = new int[,]
                        {
                            { 0, 1, 0 },
                            { 1, 1, 1 }
                        };
                        this.rowCount = 2;
                        this.columnCount = 3;
                    }
                    else if (this.rotation == 3)
                    {
                        this.shape = new int[,]
                        {
                             { 1, 0 },
                             { 1, 1 },
                             { 1, 0 }
                        };
                        this.rowCount = 3;
                        this.columnCount = 2;
                    }

                    if (!this.rotated)
                    {
                        this.code = 3;
                        this.color = Color.Yellow;
                        this.shapeImages = new Bitmap[4] { null, null, null, null };
                        this.shapeSizes = new int[4, 2];
                    }
                    break;
            }

            this.Size = new Size(this.columnCount * 30, this.rowCount * 25);
            DrawShape();
        }

        public void DrawShape()
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

            // TODO: How does a jagged array work?
            //this.shapes[this.rotation] = this.shape;
            this.shapeImages[this.rotation] = shapeImage;
            this.shapeSizes[this.rotation, 0] = this.Width;
            this.shapeSizes[this.rotation, 1] = this.Height;
            this.Image = shapeImage;
        }

        /*
        public void DrawShape()
        {
            try
            {
                this.Size = new Size(this.columnCount * 30, this.rowCount * 25);

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
                
                this.shapeImages[this.rotation] = shapeImage;
                this.shapeSizes[this.rotation, 0] = this.Width;
                this.shapeSizes[this.rotation, 1] = this.Height;
                this.Image = shapeImage;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                return;
            }
        }
        */

        public void PerformRotation()
        {
            try
            {
                this.rotation = (this.rotation + 1) % this.shapeImages.Length;

                if(!this.rotated)
                {
                    this.rotated = true;
                }

                if (this.shapeImages[this.rotation] != null)
                {
                    this.shape = this.shapes[this.rotation];
                    this.Size = new Size(shapeSizes[this.rotation, 0], shapeSizes[this.rotation, 1]);
                    this.Image = this.shapeImages[this.rotation];
                }
                else
                {
                    FormPiece();
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public int[ , ] GetShape()
        {
            return this.shape;
        }

        public int GetCode()
        {
            return this.code;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
