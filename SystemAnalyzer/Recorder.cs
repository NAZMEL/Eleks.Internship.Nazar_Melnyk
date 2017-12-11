using System;
using System.IO;

namespace SystemAnalyzer
{
    abstract class Recorder
    {
        string path = @"C:\Temp\SystemAnalyzer.txt";
        object obj = new object();

        public void RecordMessage(string content)
        {
            lock (obj)
            {
                using (StreamWriter sr = new StreamWriter(path, true, System.Text.Encoding.Default))
                {
                    if (content[0] != '-')   // for separate service sessions: "-------------------------"
                    {
                        sr.WriteLine($@"[{DateTime.Now}] --- {content}");
                    }
                    else
                    {
                        sr.WriteLine(content);
                    }
                }
            }

        }
    }
}
