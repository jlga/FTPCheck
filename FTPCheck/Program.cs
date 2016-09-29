using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseDT.Net.Ftp;
using System.IO;
using System.Threading;
using Colorful;
using System.Drawing;
using Console = Colorful.Console;

namespace FTPCheck
{
    class Program
    {
        IEnumerable<string> lines;
        string inputFile = "input.txt";
        string outputFile = "output.txt";
        static void Main(string[] args)
        {
            Program p = new Program();
        }

        Program()
        {
            Random r = new Random();
            lines = File.ReadLines(inputFile);
            Console.Out.WriteLine(lines.Count() + " " + lines.ElementAt(lines.Count()-1));
            //Thread myNewThread = new Thread(() => testServer("localhost"));
            //myNewThread.Start();
            for(int i = 0; i<lines.Count(); i=i+8)
            {
                Thread thread1 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                Thread thread2 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                Thread thread3 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                Thread thread4 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                Thread thread5 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                Thread thread6 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                Thread thread7 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                Thread thread8 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                Thread thread9 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                Thread thread10 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                Thread thread11 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                Thread thread12 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                Thread thread13 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                Thread thread14 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                Thread thread15 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                Thread thread16 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));

                thread1.Start();
                thread2.Start();
                thread3.Start();
                thread4.Start();
                thread5.Start();
                thread6.Start();
                thread7.Start();
                thread8.Start();
                thread9.Start();
                thread10.Start();
                thread11.Start();
                thread12.Start();
                thread13.Start();
                thread14.Start();
                thread15.Start();
                thread16.Start();
                Thread.Sleep(5000);
                Console.Out.WriteLine(i);
            }
            
            
            Thread.Sleep(10000);
        }

        public void testServer(string ip)
        {
            FTPConnection ftp = new FTPConnection();
            string current = "setup";
            try
            {
                Console.Out.WriteLine("Testing: " + ip);
                ftp.ServerAddress = ip;
                ftp.UserName = "anonymous";
                ftp.Password = "anonymous@";
                ftp.Timeout = 10000;
                current = "connecting";
                ftp.Connect();
                current = "uploading";
                ftp.UploadFile("test.txt", "test.txt");
                current = "testing";
                if (ftp.Exists("test.txt"))
                {
                    current = "deleting";
                    ftp.DeleteFile("test.txt");
                    Console.WriteLine("OK: " + ip, Color.LightGreen);
                    using (StreamWriter w = File.AppendText(outputFile))
                    {
                        w.WriteLine(ip);
                    }
                }
                else
                {
                    throw new Exception("File could not be uploaded" + ip);
                }
                ftp.DeleteFile("test.txt");

            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Error: " + current +" " + ex.Message);
            }
            finally
            {
                current = "closing";
                ftp.Close();
            }
        }
    }
}
