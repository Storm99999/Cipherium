using Cipherium.Core.Util.StringUtil;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cipherium.Core.Screenshot
{
    internal class Screenshotter
    {
        public static string TakeScreenshot()
        {
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(screenBounds.Width, screenBounds.Height, PixelFormat.Format32bppArgb);
            // FUCK THIS BULLSHIT
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(screenBounds.X, screenBounds.Y, 0, 0, screenBounds.Size, CopyPixelOperation.SourceCopy);
            }

            string filename = StringUtil.RandomString(20) + ".png";
            screenshot.Save(filename, ImageFormat.Png);

            return filename;
        }
    }
}
