using System;
using System.Drawing;
using System.Text;

namespace ConsoleGraphics
{
    public class Canvas(Point size) : PixelBuffer(size)
    {
        private readonly Pixel[,] _pixels = new Pixel[size.X, size.Y];

        public override void Draw()
        {
            StringBuilder sb = new(_pixels.Length);
            int writeX = 0;
            int writeY = 0;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Pixel current = _pixels[x, y];
                    sb.Append($"{PixelLeft}{PixelRight}");

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
        public override void SetPixel(int x, int y, Pixel pixel) => _pixels[x, y] = pixel;
        public override Pixel GetPixel(int x, int y) => _pixels[x, y];
    }
}