using System;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleGraphics.Rendering
{
    public readonly struct Pixel
    {
        public static Pixel White => new(ConsoleColor.White);
        public static Pixel Black => new(ConsoleColor.Black);

        public ConsoleColor BackColor { get; }
        public ConsoleColor ForeColor { get; }

        private readonly int _hash;

        public Pixel(ConsoleColor backColor, ConsoleColor foreColor)
        {
            BackColor = backColor;
            ForeColor = foreColor;            

            _hash = HashCode.Combine(BackColor, ForeColor);
        }
        public Pixel(ConsoleColor backColor) : this(backColor, ConsoleColor.White)
        {

        }

        public readonly override bool Equals([NotNullWhen(true)] object other)
        {
            if (other is not Pixel)
                return false;

            return other.GetHashCode() == GetHashCode();
        }
        public readonly override int GetHashCode() => _hash;

        public static bool operator ==(Pixel left, Pixel right) => left.Equals(right);
        public static bool operator !=(Pixel left, Pixel right) => !(left == right);
    }
}