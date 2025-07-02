namespace ConsoleGraphics.Rendering
{
    public interface IRenderer
    {
        int Width { get; } 
        int Height { get; }
        
        public char PixelLeft { get; set; }
        public char PixelRight { get; set; }

        public const int PixelWidth = 2;

        public void Render();

        public void SetSize(int width, int height);

        public void Flush(Pixel pixel);
        public void SetPixel(int x, int y, Pixel pixel);
        public Pixel GetPixel(int x, int y);
    }
}