using ConsoleGraphics.Rendering;
using System;
using System.Runtime.InteropServices;

namespace ConsoleGraphics
{
    public class NativeInterop
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
            IntPtr lpBuffer,
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

        public static CHAR_INFO ToCharInfo(ConsoleColor foreColor, ConsoleColor backColor, char character)
        {
            return new() 
            {
                Attributes = (ushort)((int)foreColor | ((int)backColor << 4)),
                UnicodeChar = character, 
            };
        }
        public static Pixel ToPixel(ushort attributes)
        {
            return new((ConsoleColor)((attributes >> 4) & 0x0F), (ConsoleColor)(attributes & 0x0F));
        }

        public static void WriteBuffer(in CHAR_INFO[] buffer, int width, int height)
        {
            IntPtr handle = GetStdHandle(STD_OUTPUT_HANDLE);

            Coord bufferSize = new() { X = (short)width, Y = (short)height };
            Coord bufferCoord = new() { X = 0, Y = 0 };
            SmallRect writeRegion = new() { Left = 0, Top = 0, Right = (short)(width - 1), Bottom = (short)(height - 1) };

            unsafe
            {
                fixed (CHAR_INFO* ptr = buffer)
                {
                    WriteConsoleOutput(handle, new IntPtr(ptr), bufferSize, bufferCoord, ref writeRegion);
                }
            }
        }
    }
}