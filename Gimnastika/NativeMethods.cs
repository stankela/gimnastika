using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using Gimnastika.Win32;

namespace Gimnastika
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int X, int Y, int Width, int Height, FlagsSetWindowPos flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowLong(IntPtr hWnd, int Index);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr hWnd, int Index, int Value);

        // BOOL ScrollWindow(HWND hWnd, int XAmount, int YAmount,
        //              const RECT *lpRect, const RECT *lpClipRect)
        [DllImport("user32.dll")]
        public static extern int ScrollWindow(IntPtr hwnd, int cx, int cy,
                                              ref RECT rectScroll,
                                              ref RECT rectClip);
    }
}

namespace Gimnastika.Win32
{
    [Flags]
    internal enum FlagsSetWindowPos : uint
    {
        SWP_NOSIZE = 0x0001,
        SWP_NOMOVE = 0x0002,
        SWP_NOZORDER = 0x0004,
        SWP_NOACTIVATE = 0x0010,
        SWP_FRAMECHANGED = 0x0020,
        SWP_NOOWNERZORDER = 0x0200,
    }

    internal enum WindowStyles : uint
    {
        WS_BORDER = 0x00800000,
    }

    internal enum WindowExStyles
    {
        WS_EX_CLIENTEDGE = 0x00000200,
    }

    internal enum GetWindowLongIndex : int
    {
        GWL_STYLE = -16,
        GWL_EXSTYLE = -20
    }

    // Define a Win32-like rectangle structure.
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

}