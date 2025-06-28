using System;
using System.Runtime.InteropServices;

namespace ConsoleGraphics
{
    public class NativeRenderer
    {
        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        public struct CHAR_INFO
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(2)] public ushort Attributes;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
            IntPtr hConsoleOutput,
            CHAR_INFO[] lpBuffer,
            Coord dwBufferSize,
            Coord dwBufferCoord,
            ref SmallRect lpWriteRegion
        );

        [StructLayout(LayoutKind.Sequential)]
        struct Coord
        {
            public short X;
            public short Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        const int STD_OUTPUT_HANDLE = -11;

        public static CHAR_INFO ToCharInfo(Pixel pixel)
        {
            return new() 
            {
                Attributes = (ushort)((int)pixel.ForeColor | ((int)pixel.BackColor << 4)),
                UnicodeChar = ' ', 
            };
        }
        public static Pixel ToPixel(ushort attributes)
        {
            return new((ConsoleColor)((attributes >> 4) & 0x0F), (ConsoleColor)(attributes & 0x0F));
        }

        public static void WriteBuffer(CHAR_INFO[] buffer, int width, int height)
        {
            IntPtr handle = GetStdHandle(STD_OUTPUT_HANDLE);

            Coord bufferSize = new() { X = (short)width, Y = (short)height };
            Coord bufferCoord = new() { X = 0, Y = 0 };
            SmallRect writeRegion = new() { Left = 0, Top = 0, Right = (short)(width - 1), Bottom = (short)(height - 1) };

            WriteConsoleOutput(handle, buffer, bufferSize, bufferCoord, ref writeRegion);
        }
    }
}