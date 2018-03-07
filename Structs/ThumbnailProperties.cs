using System.Runtime.InteropServices;

namespace DAWindower
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ThumbnailProperties
    {
        public ThumbnailFlags Flags { get; set; }
        public Rect DestinationRect { get; set; }
        public Rect SourceRect { get; set; }
        public byte Opacity { get; set; }
        public bool Visible { get; set; }
        public bool OnlyClientRect { get; set; }
    }
}
