using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

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
            Console.WriteLine("Do you want to view the state file?Y/N");
            if (Console.ReadLine().ToLower() == "y")
            {
                Process.Start("state.txt");
            }
        }

        /// <summary>
        /// Deletes the state file of a previous run
        /// </summary>
        /// <param name="path">The name of the state file</param>
        static void DeletePreviousRunData(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}