using System;
using System.Drawing;
using System.Text;

namespace ConsoleGraphics
{
    public class Canvas
    {
        public int Width { get; } = 0;
        public int Height { get; } = 0;

        private readonly Pixel[,] _pixels;

        public Canvas(Point size)
        {
            if (size.X < 1 || size.Y < 1)
                throw new ArgumentOutOfRangeException(nameof(size), "Width and height must be greater than zero");

            Width = size.X;
            Height = size.Y;

            _pixels = new Pixel[Width, Height];
        }

        public void Render()
        {
            StringBuilder sb = new(_pixels.Length);
            int writeX = 0;
            int writeY = 0;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Pixel current = _pixels[x, y];
                    sb.Append(current.Content);

                    void Write()
                    {
                        Console.BackgroundColor = current.BackColor;
                        Console.ForegroundColor = current.ForeColor;

                        Console.SetCursorPosition(writeX, writeY);
                        Console.Write(sb.ToString());
                    }

                    int nextX = x + 1;
                    int nextY = y;

                    if (nextX >= Width)
                    {
                        nextX = 0;
                        nextY++;

                        if (nextY >= Height)
                        {
                            Write();
                            break;
                        }
                        else
                        {
                            sb.AppendLine();
                        }
                    }

                    Pixel next = _pixels[nextX, nextY];

                    if (next != current)
                    {
                        Write();
                        sb.Clear();

                        writeX = nextX * 2;
                        writeY = nextY;
                    }
                }
            }
        }

        public void Fill(Pixel pixel)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    SetPixel(x, y, pixel);
                }
            }
        }

        public void SetPixel(int x, int y, Pixel pixel)
        {
            if (_pixels[x, y] != pixel)
            {
                _pixels[x, y] = pixel;
            }
        }
    }
}