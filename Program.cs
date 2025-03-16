using System;
using System.Drawing;
using System.IO;
using System.Threading;

namespace ConsoleGraphics
{
    internal class Program
    {
        private static int Width => canvas.Width;
        private static int Height => canvas.Height;
     
        private static Canvas canvas;

        static void Main(string[] args)
        {
            Console.ReadLine();
            Console.CursorVisible = false;
            
            Point size = new(Console.WindowWidth / 2, Console.WindowHeight - 1);
            canvas = new(size);

            GameOfLife game = new(Width, Height);

            while (true)
            {
                canvas.Fill(new(ConsoleColor.Blue));

                game.Step();
                game.Draw(canvas);

                canvas.RenderNative();
            }
        }
    }

    //an example
    public class GameOfLife
    {
        private int Width => cells.GetLength(0);
        private int Height => cells.GetLength(1);
        
        private bool[,] cells, nextCells;

        public GameOfLife(int width, int height)
        {
            Random random = new();
            cells = new bool[width, height];
            nextCells = new bool[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cells[x, y] = random.NextSingle() > 0.5f;
                }
            }
        }

        public void Step()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int neighbors = GetNeighborsCount(x, y);

                    bool isAlive = IsCellAlive(x, y);

                    if (isAlive)
                    {
                        if (neighbors < 2 || neighbors > 3)
                            nextCells[x, y] = false;
                        else 
                            nextCells[x, y] = true;
                    }
                    else if (neighbors == 3)
                    {
                        nextCells[x, y] = true;
                    }
                    else 
                        nextCells[x, y] = cells[x, y];
                }
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    cells[x, y] = nextCells[x, y];
                }
            }
        }

        public int GetNeighborsCount(int rootX, int rootY)
        {
            int count = 0;

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    int neighborX = rootX + x;
                    int neighborY = rootY + y;

                    if (neighborX >= Width || neighborY >= Height || neighborX < 0 || neighborY < 0)
                        continue;

                    if (x == 0 && y == 0)
                        continue;

                    if (IsCellAlive(neighborX, neighborY))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private bool IsCellAlive(int x, int y) => cells[x, y]; 

        public ConsoleColor GetColor(int x, int y)
        {
            return cells[x, y] ? ConsoleColor.White : ConsoleColor.Blue;
        }

        public void Draw(Canvas canvas)
        {
            for (int y = 0; y < canvas.Height; y++)
            {
                for (int x = 0; x < canvas.Width; x++)
                {
                    canvas.SetPixel(x, y, new Pixel(GetColor(x, y)));
                }
            }
        }
    }
}