using System.Runtime.InteropServices;
using Echo.Definitions;

namespace Echo.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ThumbnailProperties
    {
        public ThumbnailFlags Flags { get; set; }
        public Rect DestinationRect { get; set; }
        public Rect SourceRect { get; set; }
        public byte Opacity { get; set; }
        public bool Visible { get; set; }
        public bool OnlyClientRect { get; set; }
    }
}