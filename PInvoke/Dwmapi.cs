using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAWindower
{
    internal static class Dwmapi
    {
        [DllImport("dwmapi.dll")]
        internal static extern int DwmRegisterThumbnail(IntPtr dest, IntPtr src, out IntPtr thumb);
        [DllImport("dwmapi.dll")]
        internal static extern int DwmUpdateThumbnailProperties(IntPtr hThumb, ref ThumbnailProperties props);
        [DllImport("dwmapi.dll")]
        internal static extern int DwmUnregisterThumbnail(IntPtr thumb);
        [DllImport("dwmapi.dll")]
        internal static extern int DwmQueryThumbnailSourceSize(IntPtr thumb, out ThumbnailSize size);
    }
}
