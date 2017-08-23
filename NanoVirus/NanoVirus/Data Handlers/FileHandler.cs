using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NanoVirus
{
    public static class FileHandler
    {
        public static void Append(string data, string filePath)
        {
            FileStream streamFile = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(streamFile);
            writer.Write(data);
            writer.Close();
            streamFile.Close();
        }
    }
}