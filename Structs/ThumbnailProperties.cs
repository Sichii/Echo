using System.Runtime.InteropServices;

namespace DAWindower
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ThumbnailProperties
    {
        public int Flags;
        public Rect DestinationRect;
        public Rect SourceRect;
        public byte Opacity;
        public bool Visible;
        public bool OnlyClientRect;
    }
}
