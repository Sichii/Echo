using System.Drawing;
using System.Runtime.InteropServices;

namespace Echo
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Rect
    {
        public int Left, Top, Right, Bottom;

        public Rect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Rect(Rectangle r) 
            : this(r.Left, r.Top, r.Right, r.Bottom)
        {
        }

        public int X
        {
            get { return Left; }
            set
            {
                Right -= (Left - value);
                Left = value;
            }
        }

        public int Y
        {
            get { return Top; }
            set
            {
                Bottom -= (Top - value);
                Top = value;
            }
        }

        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }

        public Point Location
        {
            get { return new Point(Left, Top); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Size Size
        {
            get { return new Size(Width, Height); }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public static implicit operator Rectangle(Rect r) => new Rectangle(r.Left, r.Top, r.Width, r.Height);
        public static implicit operator Rect(Rectangle r) => new Rect(r);
        public static bool operator ==(Rect r1, Rect r2) => r1.Equals(r2);
        public static bool operator !=(Rect r1, Rect r2) => !r1.Equals(r2);
        public bool Contains(int x, int y) => x >= Left && x <= Right && y >= Top && y <= Bottom;
        public bool Equals(Rect r) => r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;

        public override bool Equals(object obj)
        {
            if (obj is Rect)
                return Equals((Rect)obj);
            else if (obj is Rectangle)
                return Equals(new Rect((Rectangle)obj));
            return false;
        }

        public override int GetHashCode() => ((Rectangle)this).GetHashCode();
        public override string ToString() => $@"{{Left={Left},Top={Top},Right={Right},Bottom={Bottom}}}";
    }
}
