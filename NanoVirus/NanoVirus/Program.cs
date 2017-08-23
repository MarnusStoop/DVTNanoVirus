using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NanoVirus
{
    class Program
    {
        static void Main(string[] args)
        {
            string appStateFilePath = "State.txt";
            DeletePreviousRunData(appStateFilePath);
            Body b = new Body(100,5,appStateFilePath);
            b.GenerateBody();
            b.StartVirus();
            Console.ReadLine();
        }

        static void DeletePreviousRunData(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}