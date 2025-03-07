using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGraphics
{
    public readonly struct Pixel
    {
        public static Pixel White => new(ConsoleColor.White);
        public static Pixel Black => new(ConsoleColor.Black);

        public const string DefaultContent = "  ";

        public string Content { get; }
        public ConsoleColor BackColor { get; }
        public ConsoleColor ForeColor { get; }

        private readonly int _hash;

        public Pixel(ConsoleColor backColor, ConsoleColor foreColor, string content)
        {
            BackColor = backColor;
            ForeColor = foreColor;
            Content = content;

            _hash = HashCode.Combine(Content, BackColor, ForeColor);
        }
        public Pixel(ConsoleColor backColor) : this(backColor, ConsoleColor.White, DefaultContent)
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