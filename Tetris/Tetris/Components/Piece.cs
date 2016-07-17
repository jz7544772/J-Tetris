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
        string name;
        Color color;
        int code;

        int rowCount;
        int columnCount;

        int[][,] rotations;
        Bitmap[] rotationImages;
        int rotationIndex;
        int[,] rotation;

        public Piece(string name, int rotationIndex = 0)
        {
            this.BackColor = Color.Transparent;

            this.name = name;
            this.rotationIndex = rotationIndex;
            this.Location = new Point(Properties.Settings.Default.gridWidth, 0);

            this.SetRotations();

            InitializeComponent();
        }

        public void SetRotations()
        {
            switch (this.name)
            {
                case "stick":
                    this.code = 1;
                    this.color = Color.Cyan;
                    SetDimension(4, 4);
                    this.rotations = new int[4][,];
                    this.rotations[0] = new int[,]
                    {
                        { 0, 0, 0, 0 },
                        { 1, 1, 1, 1 },
                        { 0, 0, 0, 0 },
                        { 0, 0, 0, 0 }
                    };
                    this.rotations[1] = new int[,]
                    {
                        { 0, 0, 1, 0 },
                        { 0, 0, 1, 0 },
                        { 0, 0, 1, 0 },
                        { 0, 0, 1, 0 }
                    };
                    this.rotations[2] = new int[,]
                    {
                        { 0, 0, 0, 0 },
                        { 0, 0, 0, 0 },
                        { 1, 1, 1, 1 },
                        { 0, 0, 0, 0 }
                    };
                    this.rotations[3] = new int[,]
                    {
                        { 0, 1, 0, 0 },
                        { 0, 1, 0, 0 },
                        { 0, 1, 0, 0 },
                        { 0, 1, 0, 0 }
                    };
                    break;
                case "square":
                    this.code = 2;
                    this.color = Color.LightGray;
                    SetDimension(2, 2);
                    this.rotations = new int[1][,];
                    this.rotations[0] = new int[,] {
                        { 1, 1 },
                        { 1, 1 }
                    };
                    break;
                case "tee":
                    this.code = 3;
                    this.color = Color.Yellow;
                    SetDimension(3, 3);
                    this.rotations = new int[4][,];
                    this.rotations[0] = new int[,]
                    {
                       { 0, 1, 0 },
                       { 1, 1, 1 },
                       { 0, 0, 0 }
                    };
                    this.rotations[1] = new int[,]
                    {
                       { 0, 1, 0 },
                       { 0, 1, 1 },
                       { 0, 1, 0 }
                    };
                    this.rotations[2] = new int[,]
                    {
                       { 0, 0, 0 },
                       { 1, 1, 1 },
                       { 0, 1, 0 }
                    };
                    this.rotations[3] = new int[,]
                    {
                       { 0, 1, 0 },
                       { 1, 1, 0 },
                       { 0, 1, 0 }
                    }; 
                    break;
                case "ess":
                    this.code = 4;
                    this.color = Color.Lime;
                    SetDimension(3, 3);
                    this.rotations = new int[4][,];
                    this.rotations[0] = new int[,]
                    {
                       { 0, 1, 1 },
                       { 1, 1, 0 },
                       { 0, 0, 0 }
                    };
                    this.rotations[1] = new int[,]
                    {
                       { 0, 1, 0 },
                       { 0, 1, 1 },
                       { 0, 0, 1 }
                    };
                    this.rotations[2] = new int[,]
                    {
                       { 0, 0, 0 },
                       { 0, 1, 1 },
                       { 1, 1, 0 }
                    };
                    this.rotations[3] = new int[,]
                    {
                       { 1, 0, 0 },
                       { 1, 1, 0 },
                       { 0, 1, 0 }
                    };
                    break;
                case "zed":
                    this.code = 5;
                    this.color = Color.Red;
                    SetDimension(3, 3);
                    this.rotations = new int[4][,];
                    this.rotations[0] = new int[,]
                   {
                       { 1, 1, 0 },
                       { 0, 1, 1 },
                       { 0, 0, 0 }
                   };
                    this.rotations[1] = new int[,]
                    {
                       { 0, 0, 1 },
                       { 0, 1, 1 },
                       { 0, 1, 0 }
                    };
                    this.rotations[2] = new int[,]
                    {
                       { 0, 0, 0 },
                       { 1, 1, 0 },
                       { 0, 1, 1 }
                    };
                    this.rotations[3] = new int[,]
                    {
                       { 0, 1, 0 },
                       { 1, 1, 0 },
                       { 1, 0, 0 }
                    };
                    break;
                case "jay":
                    this.code = 6;
                    this.color = Color.Purple;
                    SetDimension(3, 3);
                    this.rotations = new int[4][,];
                    this.rotations[0] = new int[,]
                   {
                       { 1, 0, 0 },
                       { 1, 1, 1 },
                       { 0, 0, 0 }
                   };
                    this.rotations[1] = new int[,]
                    {
                       { 0, 1, 1 },
                       { 0, 1, 0 },
                       { 0, 1, 0 }
                    };
                    this.rotations[2] = new int[,]
                    {
                       { 0, 0, 0 },
                       { 1, 1, 1 },
                       { 0, 0, 1 }
                    };
                    this.rotations[3] = new int[,]
                    {
                       { 0, 1, 0 },
                       { 0, 1, 0 },
                       { 1, 1, 0 }
                    };
                    break;
                case "el":
                    this.code = 6;
                    this.color = Color.Magenta;
                    SetDimension(3, 3);
                    this.rotations = new int[4][,];
                    this.rotations[0] = new int[,]
                   {
                       { 0, 0, 1 },
                       { 1, 1, 1 },
                       { 0, 0, 0 }
                   };
                    this.rotations[1] = new int[,]
                    {
                       { 0, 1, 0 },
                       { 0, 1, 0 },
                       { 0, 1, 1 }
                    };
                    this.rotations[2] = new int[,]
                    {
                       { 0, 0, 0 },
                       { 1, 1, 1 },
                       { 1, 0, 0 }
                    };
                    this.rotations[3] = new int[,]
                    {
                       { 1, 1, 0 },
                       { 0, 1, 0 },
                       { 0, 1, 0 }
                    };
                    break;
            }
            this.rotationImages = new Bitmap[this.rotations.Length];
            this.rotation = this.rotations[this.rotationIndex];
            SetSize();
            DrawShape();
        }

        private void SetDimension(int rowCount, int columnCount)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;
        }

        private void SetSize()
        {
            this.Size = new Size(this.columnCount * Properties.Settings.Default.gridWidth,
                                  this.rowCount * Properties.Settings.Default.gridHeight);
        }

        public void DrawShape()
        {
            if(this.rotationImages[this.rotationIndex] != null)
            {
                this.Image = this.rotationImages[this.rotationIndex];
            }
            else
            {
                Bitmap rotationImage = new Bitmap(this.Width, this.Height);
                Pen shapePen = new Pen(Color.White, 1);

                SolidBrush shapeBrush = new SolidBrush(this.color);
                Rectangle tempRect;

                using (Graphics g = Graphics.FromImage(rotationImage))
                {
                    for (int row = 0; row < this.rowCount; row++)
                    {
                        for (int column = 0; column < this.columnCount; column++)
                        {
                            if (this.rotation[row, column] == 1)
                            {
                                tempRect = new Rectangle(
                                    column * Properties.Settings.Default.gridWidth,
                                    row * Properties.Settings.Default.gridHeight,
                                    Properties.Settings.Default.gridWidth,
                                    Properties.Settings.Default.gridHeight);

                                g.FillRectangle(shapeBrush, tempRect);
                                g.DrawRectangle(shapePen, tempRect);
                            }
                        }
                    }
                    this.rotationImages[this.rotationIndex] = rotationImage;
                    this.Image = rotationImage;
                }
            }
        }

        public void Rotate()
        {
            try
            {
                this.rotationIndex = (this.rotationIndex + 1) % this.rotations.Length;
                this.rotation = this.rotations[this.rotationIndex];
                this.DrawShape();
            } catch(Exception ex)
            {
                MessageBox.Show(
                    "Piece.Rotate: " + "\n" +
                    ex.Message + "\n" +
                    ex.StackTrace
                );
            }
        }

        public int[ , ] GetRotation()
        {
            return this.rotation;
        }
        public int[ , ] GetNextRotation()
        {
            int nextRotationIndex = (this.rotationIndex + 1) % this.rotations.Length;
            return this.rotations[nextRotationIndex];
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
