using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FoxExtensions;
using System.Threading;
namespace WritingShared5
{
    public static class DoStuff
    {
        public static string file;
        public static string folder;
        public static StreamWriter sw;
        
        public static FileStream fsr;
        public static long InfoBytes;
        public static long ReadAllBytes;
        public static FileStream fsw;
        public static bool tog;
        public static FileStream openReader;
        public static long openReaderBytes;
        public static void setup()
        {
            string root = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;

            if (!Directory.Exists(root + "\\FileTest88"))
                Directory.CreateDirectory(root + "\\FileTest88");
            folder = root + "\\FileTest88";
            file = folder + "\\test.txt";
            if(File.Exists(file))
                File.Delete(file);
            var sr = File.CreateText(file);
            sr.Close();
            sr.Dispose();
            using (var fsw = new FileStream(file, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            {
                fsw.Write("test".ToBytes());
            }
            fsr = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            fsr.Close();
            fsr.Dispose();
            tog = true;

        }
        public static void readFileInfo()
        {
            var directory = new DirectoryInfo(folder);
            var files = directory.GetFiles("test.txt");
            InfoBytes = files[0].Length;

        }

        public static void ReadAllFile()
        {
            if(openReader == null)
            {
               openReader = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            { 
                ReadAllBytes = fs.Length;
            }
            openReaderBytes = openReader.Length;
                
        }
        public static void WriteSomeBytes()
        {
            if (fsw == null) {
                fsw = new FileStream(file, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            }
            
            
                fsw.Write(" some".ToBytes());

            if (tog)
            {
                fsw.Flush();
                tog = false;
            }
            else
            {
                tog = true;
            }
        }

        public static void StartTest()
        {
            bool stop = false;
            while (!stop) {
                if (Console.KeyAvailable)
                {
                    Console.WriteLine("key is available");
                    if (Console.ReadKey(true).Key == ConsoleKey.Spacebar)
                    {

                        stop = true;
                    }
                }

                if (!stop)
                {
                    Thread.Sleep(100);
                    WriteSomeBytes();
                    readFileInfo();
                    ReadAllFile();
                    Console.WriteLine($"readAllFile bytes: FileInfoBytes {InfoBytes} -- using: {ReadAllBytes}  OpenReader {openReaderBytes} flushed: {tog}");
                   

                }
               }
            
        }
    }
}
