using System;
using System.Drawing;
using System.Text;
using static ConsoleGraphics.InteropRenderer;

namespace ConsoleGraphics
{
    public class Canvas
    {
        public int Width { get; } 
        public int Height { get; } 
        public char PixelLeft { get; } = ' ';
        public char PixelRight { get; } = ' ';

        private readonly Pixel[,] _pixels;
        private readonly CHAR_INFO[] _infos; 

        public Canvas(Point size)
        {
            if (size.X < 1 || size.Y < 1)
                throw new ArgumentOutOfRangeException(nameof(size), "Width and height must be greater than zero");

            Width = size.X;
            Height = size.Y;

            _pixels = new Pixel[Width, Height];
            _infos = new CHAR_INFO[Width * 2 * Height];
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
        public void RenderNative() => WriteBuffer(_infos, Width * 2, Height);

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
            _pixels[x, y] = pixel;

            var left = ToCharInfo(pixel);
            var right = ToCharInfo(pixel);

            left.UnicodeChar = PixelLeft;
            right.UnicodeChar = PixelRight;

            _infos[(x * 2) + y * (Width * 2)] = left;
            _infos[(x * 2) + 1 + y * (Width * 2)] = right;
        }

        public Pixel GetPixel(int x, int y) => _pixels[x, y];
    }
}