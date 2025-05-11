using System;
using System.Drawing;
using System.Net;
using System.Text;
using static ConsoleGraphics.InteropRenderer;

namespace ConsoleGraphics
{
    public abstract class PixelBuffer
    {
        public int Width { get; }
        public int Height { get; }
        public int OriginalWidth { get; }

        public char PixelLeft { get; } = ' ';
        public char PixelRight { get; } = ' ';

        protected PixelBuffer(Point size) 
        {
            if (size.X < 1 || size.Y < 1)
                throw new ArgumentOutOfRangeException(nameof(size), "Width and height must be greater than zero");

            Width = size.X;
            OriginalWidth = size.X * 2;
            Height = size.Y;
        }

        public abstract void Draw();

        public void Flush(Pixel pixel)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    SetPixel(x, y, pixel);
                }
            }
        }
        public abstract void SetPixel(int x, int y, Pixel pixel);
        public abstract Pixel GetPixel(int x, int y);
    }
}