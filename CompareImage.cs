using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Security.Cryptography;


namespace IntegrationTest
{
    class CompareImage
    {
        /// <summary>
        /// method for comparing 2 images to see if they are the same. First
        /// we convert both images to a byte array, we then get their hash (their
        /// hash should match if the images are the same), we then loop through
        /// each item in the hash comparing with the 2nd Bitmap
        /// </summary>
        /// <param name="bmp1"></param>
        /// <param name="bmp2"></param>
        /// <returns></returns>

        static String classname = "CompareImage:";

        public bool compare(String file1, String file2)
        {
            Bitmap bm1 = new Bitmap(file1);
            Bitmap bm2 = new Bitmap(file2);
            bool compared = doImagesMatch(ref bm1, ref bm2);
            Launcher.logcontent += classname + "-c-" + compared + "\r\n";
            return compared;
        }

        public bool doImagesMatch(ref Bitmap bmp1, ref Bitmap bmp2)
        {
            try
            {
                //create instance or System.Drawing.ImageConverter to convert
                //each image to a byte array
                ImageConverter converter = new ImageConverter();
                //create 2 byte arrays, one for each image
                byte[] imgBytes1 = new byte[1];
                byte[] imgBytes2 = new byte[1];

                //convert images to byte array
                imgBytes1 = (byte[])converter.ConvertTo(bmp1, imgBytes2.GetType());
                imgBytes2 = (byte[])converter.ConvertTo(bmp2, imgBytes1.GetType());

                //now compute a hash for each image from the byte arrays
                SHA256Managed sha = new SHA256Managed();
                byte[] imgHash1 = sha.ComputeHash(imgBytes1);
                byte[] imgHash2 = sha.ComputeHash(imgBytes2);

                //now let's compare the hashes
                for (int i = 0; i < imgHash1.Length && i < imgHash2.Length; i++)
                {
                    //whoops, found a non-match, exit the loop
                    //with a false value
                    if (!(imgHash1[i] == imgHash2[i]))
                        return false;
                }
            }
            catch (Exception ex)
            {
                Launcher.logcontent += classname + "-dIM-" + false + (ex.Message) + "\r\n";
                return false;
            }
            //we made it this far so the images must match
            return true;
        }
    }
}
