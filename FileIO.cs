using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace IntegrationTest
{
    class FileIO
    {
        
public FileIO()
{
    if (!Directory.Exists(@"\TestbGUI"))
        Directory.CreateDirectory(@"\TestbGUI");
    if (!Directory.Exists(@"\TestbGUI\info"))
         Directory.CreateDirectory(@"\TestbGUI\info");
    if (!Directory.Exists(@"\TestbGUI\log"))
        Directory.CreateDirectory(@"\TestbGUI\log");
}



        public void Write(String filename, String content, String directory)
        {

            if (Launcher.printme || directory != "log")//AM: can shut off logging but maintain any other files writing like "info"
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(content);
                StreamWriter outfile = null; 
                if (directory == "uialogger")
                {
                    outfile = new StreamWriter(/*Launcher.logdirectory + "\\" + directory + "\\" + */filename);
                }
                else
                {
                    outfile = new StreamWriter(Launcher.logdirectory + "\\" + directory + "\\" + filename);
                }
                outfile.Write(sb.ToString());
                outfile.Close();
            }

 

        }

        public void Append(String filename, String content,String directory)
        {

            if (Launcher.printme || directory != "log")//AM: can shut off logging but maintain any other files writing like "info"
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(content);
                StreamWriter outfile = new StreamWriter(Launcher.logdirectory + "\\" + directory + "\\" + filename, true);
                outfile.Write(sb.ToString());
                outfile.Close();
            }



        }

        public String Read(String filename)
        {


            String content = "";
            StreamReader sr = null;
            try
            {
                /*StreamReader*/ sr = new StreamReader(filename);
            content = sr.ReadToEnd();
            sr.Close();
            }
            catch
            {
                content="File Not Found";
            }

 
            return content;
        }

        public System.IO.FileInfo[] ListFiles(String filepath)
        {           
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filepath);
            System.IO.FileInfo[] fi = di.GetFiles("*.rpt");

            System.IO.FileInfo fiLatest = di.GetFiles().OrderByDescending(fil => fil.CreationTime).First();

            return fi;
        }

        public String GetLatestFile(String filepath)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filepath);
            //System.IO.FileInfo[] fi = di.GetFiles("*.rpt");

            System.IO.FileInfo fiLatest = di.GetFiles().OrderByDescending(fil => fil.CreationTime).First();
            string file = Path.GetFileName(fiLatest.FullName);

            return file;
        }

        public String CopyFile(String filesource, String filetarget)
        {
            String result = "";
            
            try
            {
                //creates and fill the bytes array with the data.
                byte[] readBuffer = System.IO.File.ReadAllBytes(filesource);

                //finally we do the writing of the bytes array back to a new file.
                //(this will overwrite any existing file matching the filename).
                System.IO.File.WriteAllBytes(filetarget, readBuffer);
                Console.WriteLine("FILE SUCCESSFULLY CREATED");
                result = "FILE SUCCESSFULLY CREATED";

            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
                result = e.Message;
            }

            return result;

        }
    }
}
