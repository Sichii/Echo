using System.Drawing;
using System.Runtime.InteropServices;

namespace Echo.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct Rect
{
    #region Do Not ReOrder

    public int Left, Top, Right, Bottom;

    public int X
    {
        get => Left;
        set
        {
            Right -= Left - value;
            Left = value;
        }
    }

    public int Y
    {
        get => Top;
        set
        {
            Bottom -= Top - value;
            Top = value;
        }
    }

    public int Height
    {
        get => Bottom - Top;
        set => Bottom = value + Top;
    }

    public int Width
    {
        get => Right - Left;
        set => Right = value + Left;
    }

    public Point Location
    {
        get => new(Left, Top);
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }

    public Size Size
    {
        get => new(Width, Height);
        set
        {
            Width = value.Width;
            Height = value.Height;
        }
    }

    public static implicit operator Rectangle(Rect r) => new(r.Left, r.Top, r.Width, r.Height);
    public static implicit operator Rect(Rectangle r) => new(r);
    public static bool operator ==(Rect r1, Rect r2) => r1.Equals(r2);
    public static bool operator !=(Rect r1, Rect r2) => !r1.Equals(r2);

    public Rect(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public Rect(Rectangle r)
        : this(r.Left, r.Top, r.Right, r.Bottom) { }

    public bool Contains(int x, int y) => x >= Left && x <= Right && y >= Top && y <= Bottom;
    public bool Equals(Rect r) => r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;

    public override bool Equals(object obj) =>
        obj switch
        {
            Rect rect           => Equals(rect),
            Rectangle rectangle => Equals(new(rectangle)),
            _                   => false
        };

    public override int GetHashCode() => ((Rectangle)this).GetHashCode();
    public override string ToString() => $@"{{Left={Left},Top={Top},Right={Right},Bottom={Bottom}}}";

    #endregion
}