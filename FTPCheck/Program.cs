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
        //Config
        const int THREAD_AMOUNT = 64;

        
        IEnumerable<string> lines;
        string inputFile = "input.txt";
        string outputFile = "output.txt";

        List<string> checkedServers;
        List<string> goodServers;

        int okservers = 0;
        static void Main(string[] args)
        {
            Program p = new Program();
        }

        Program()
        {
            try
            {
                okservers = File.ReadLines(outputFile).Count();
            }
            catch
            {
                okservers = 0;
            }
            Random r = new Random();
            lines = File.ReadLines(inputFile);
            Console.Out.WriteLine(lines.Count() + " " + lines.ElementAt(lines.Count()-1));
            for (int i = 1; i < lines.Count(); i++)
            {
                Thread thread1 = new Thread(() => testServer(lines.ElementAt(r.Next(0, lines.Count()))));
                thread1.Start();
                Thread.Sleep(1);
                if(i % THREAD_AMOUNT == 0)
                {
                    Properties.Settings.Default["ServersChecked"] = Convert.ToInt32(Properties.Settings.Default["ServersChecked"].ToString()) + THREAD_AMOUNT;
                    Properties.Settings.Default.Save();
                    Console.Write(Properties.Settings.Default["ServersChecked"].ToString() + " ", Color.Yellow);
                    Console.WriteLine(okservers, Color.Lime);
                    Thread.Sleep(5000);
                }
            }


                Thread.Sleep(10000);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
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
                ftp.Timeout = 5000;
                current = "connecting";
                ftp.Connect();
                current = "uploading";
                ftp.UploadFile("test.txt", "test.txt");
                current = "testing";
                if (ftp.Exists("test.txt"))
                {
                    current = "deleting";
                    ftp.DeleteFile("test.txt");
                    Console.WriteLine("OK: " + ip, Color.Lime);
                    okservers++;
                    goodServers.Add(ip);
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
                Console.WriteLine("Error: " + current +" " + ex.Message);
            }
            finally
            {
                current = "closing";
                ftp.Close();
            }
        }
    }
}
