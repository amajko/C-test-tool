using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
         using System.Drawing;
   using System.Drawing.Imaging;
   using System.Windows.Forms;
using System.IO;


namespace IntegrationTest
{
    class ScreenCapture
    {
        public static String imagefolder = "image";
        static String classname = "ScreenCapture:";

        public ScreenCapture()
{
    if (!Directory.Exists(@"\TestbGUI\image"))
        Directory.CreateDirectory(@"\TestbGUI\image");
}

        public String GetDesktopImage(string imageFilePath)
        {
            // Declare a structure for the width and height of the desktop
            Rectangle rect = Screen.GetBounds(Point.Empty);

            // Declare a new instance of the bitmap class
            Bitmap image = new Bitmap(rect.Width, rect.Height);

            // Capture the desktop
            using (Graphics desktopImage = Graphics.FromImage(image))
            {
                desktopImage.CopyFromScreen(Point.Empty, Point.Empty, rect.Size);
            }

            Launcher.logcontent += classname + "-GDI-" + "Saving " + Launcher.logdirectory + "\\" + imagefolder + "\\" + Launcher.logname + "_" + imageFilePath + ".jpg" + "\r\n";

            // Save the image to the specified path & filename
            image.Save(Launcher.logdirectory + "\\" + imagefolder + "\\" + Launcher.logname + "_" + imageFilePath + ".jpg", ImageFormat.Jpeg);
            // And Save the image to the STANDARD path & filename for attachemnt to other workflows (like AHP)
            image.Save(Launcher.logdirectory + "\\" + imagefolder + "\\" /*+ Launcher.logname + "_" + imageFilePath*/ + "latestscreencapture" + ".jpg", ImageFormat.Jpeg);

            return Launcher.logdirectory + "\\" + imagefolder + "\\" + Launcher.logname + "_" + imageFilePath + ".jpg";
        }
 
        public String GetDesktopImage(string imageFilePath, String number)
    {
      // Declare a structure for the width and height of the desktop
     Rectangle rect = Screen.GetBounds(Point.Empty);
    
     // Declare a new instance of the bitmap class
     Bitmap image = new Bitmap(rect.Width, rect.Height);
    
     // Capture the desktop
     using (Graphics desktopImage = Graphics.FromImage(image))
     {
       desktopImage.CopyFromScreen(Point.Empty, Point.Empty, rect.Size);
     }

     Launcher.logcontent += classname + "-GDI-" + "Saving " + Launcher.logdirectory + "\\" + imagefolder + "\\" + Launcher.logname + "_" + imageFilePath + ".jpg" + "\r\n";

     // Save the image to the specified path & filename
     image.Save(Launcher.logdirectory + "\\" + imagefolder + "\\"  + Launcher.logname + "_" + imageFilePath + ".jpg", ImageFormat.Jpeg);
     // And Save the image to the STANDARD path & filename for attachemnt to other workflows (like AHP)
     image.Save(Launcher.logdirectory + "\\" + imagefolder + "\\" /*+ Launcher.logname + "_" + imageFilePath*/ + "latestscreencapture" + number + ".jpg", ImageFormat.Jpeg);

     return Launcher.logdirectory + "\\" + imagefolder + "\\" + Launcher.logname + "_" + imageFilePath + ".jpg";
   }
   }
}
