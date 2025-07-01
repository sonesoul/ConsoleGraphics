using System.Drawing;
using static ConsoleGraphics.NativeInterop;

namespace ConsoleGraphics.Rendering
{
    public class NativeCanvas(Point size) : PixelBuffer(size)
    {
        private readonly CHAR_INFO[] _infos = new CHAR_INFO[size.X * 2 * size.Y];

        public override void Draw() => WriteBuffer(_infos, OriginalWidth, Height);
        public override void SetPixel(int x, int y, Pixel pixel)
        {
            var foreColor = pixel.ForeColor;
            var backColor = pixel.BackColor;

            CHAR_INFO left = ToCharInfo(foreColor, backColor, PixelLeft);
            CHAR_INFO right = ToCharInfo(foreColor, backColor, PixelRight);

            _infos[x * 2 + y * OriginalWidth] = left;
            _infos[x * 2 + 1 + y * OriginalWidth] = right;
        }
        public override Pixel GetPixel(int x, int y) => ToPixel(_infos[y * Width + x].Attributes);
    }
}