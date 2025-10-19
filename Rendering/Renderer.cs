using System;
using System.Text;

namespace ConsoleGraphics.Rendering
{
    public class Renderer : IRenderer
    {
        public int Width { get; private set; } = 1;
        public int Height { get; private set; } = 1;

        public char PixelLeft { get; set; } = ' ';
        public char PixelRight { get; set; } = ' ';

        public const int PixelWidth = 2;

        private Pixel[,] _pixels = new Pixel[1, 1];

        private readonly StringBuilder stringBuilder = new();

        public Renderer(int width, int height) => SetSize(width, height);

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
            
            _pixels = new Pixel[width, height];
        }

        public void Render()
        {
            stringBuilder.Clear();

            int writeX = 0;
            int writeY = 0;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Pixel current = _pixels[x, y];
                    stringBuilder.Append($"{PixelLeft}{PixelRight}");

                    void Write()
                    {
                        Console.BackgroundColor = current.BackColor;
                        Console.ForegroundColor = current.ForeColor;

                        Console.SetCursorPosition(writeX, writeY);
                        Console.Write(stringBuilder.ToString());
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
                            return;
                        }
                        else
                        {
                            stringBuilder.AppendLine();
                        }
                    }

                    Pixel next = _pixels[nextX, nextY];

                    if (next != current)
                    {
                        Write();
                        stringBuilder.Clear();

                        writeX = nextX * 2;
                        writeY = nextY;
                    }
                }
            }
        }

        public void SetPixel(int x, int y, in Pixel pixel) => _pixels[x, y] = pixel;
        public Pixel GetPixel(int x, int y) => _pixels[x, y];

        public void Flush(in Pixel pixel)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    SetPixel(x, y, pixel);
                }
            }
        }
    }
}