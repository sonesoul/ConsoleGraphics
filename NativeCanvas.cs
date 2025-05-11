using System.Drawing;
using static ConsoleGraphics.InteropRenderer;

namespace ConsoleGraphics
{
    public class NativeCanvas(Point size) : PixelBuffer(size)
    {
        private readonly CHAR_INFO[] _infos = new CHAR_INFO[size.X * 2 * size.Y];

        public override void Draw() => WriteBuffer(_infos, OriginalWidth, Height);
        public override void SetPixel(int x, int y, Pixel pixel)
        {
            CHAR_INFO left = ToCharInfo(pixel);
            CHAR_INFO right = ToCharInfo(pixel);

            left.UnicodeChar = PixelLeft;
            right.UnicodeChar = PixelRight;

            _infos[(x * 2) + y * OriginalWidth] = left;
            _infos[(x * 2) + 1 + y * OriginalWidth] = right;
        }
        public override Pixel GetPixel(int x, int y) => ToPixel(_infos[y * Width + x].Attributes);
    }
}
