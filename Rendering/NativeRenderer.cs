using static ConsoleGraphics.NativeInterop;

namespace ConsoleGraphics.Rendering
{
    public class NativeRenderer : IRenderer
    {
        public int Width { get; private set; } = 1;
        public int Height { get; private set; } = 1;

        public char PixelLeft { get; set; } = ' ';
        public char PixelRight { get; set; } = ' ';

        public const int PixelWidth = 2;

        private CHAR_INFO[] _infos;
        private int _lineWidth;

        public NativeRenderer(int width, int height) => SetSize(width, height);

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
            _lineWidth = Width * PixelWidth;

            _infos = new CHAR_INFO[_lineWidth * height];
        }

        public void Render() => WriteBuffer(_infos, _lineWidth, Height);

        public void SetPixel(int x, int y, in Pixel pixel)
        {
            var foreColor = pixel.ForeColor;
            var backColor = pixel.BackColor;

            x *= PixelWidth;
            int width = y * _lineWidth;

            _infos[x + width] = ToCharInfo(foreColor, backColor, PixelLeft);
            _infos[x + 1 + width] = ToCharInfo(foreColor, backColor, PixelRight);
        }
        public Pixel GetPixel(int x, int y) => ToPixel(_infos[y * _lineWidth + x].Attributes);

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
