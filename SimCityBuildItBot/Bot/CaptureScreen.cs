using Common.Logging;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SimCityBuildItBot.Bot
{
    public class CaptureScreen
    {
        private readonly ILog log;

        public CaptureScreen(ILog log)
        {
            this.log = log;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        public Bitmap SnapShot(int x, int y, Size size)
        {
            Process proc = MemuChooser.GetMemuProcess();
            if (proc == null)
            {
                System.Diagnostics.Debug.WriteLine("Can't find process: ");
                log.Debug("Can't find process: ");
                return null;
            }

            Rect rect = new Rect();
            IntPtr error = GetWindowRect(proc.MainWindowHandle, ref rect);

            // adb shell am display-size 1920x1080

            if ((rect.bottom - rect.top) != 1080)
            {
                log.Debug("height of window must be 1080, it is "+(rect.bottom - rect.top));
            }

            if ((rect.right - rect.left) != 1920)
            {
                log.Debug("width of window must be 1920, it is "+( rect.right - rect.left));
            }

            // sometimes it gives error.
            while (error == (IntPtr)0)
            {
                error = GetWindowRect(proc.MainWindowHandle, ref rect);
            }

            Bitmap bmp = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            Graphics.FromImage(bmp).CopyFromScreen(rect.left + x, rect.top + y, 0, 0, size, CopyPixelOperation.SourceCopy);

            return bmp;
        }
    }
}