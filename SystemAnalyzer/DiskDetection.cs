using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;//remove unnecessary usings

namespace SystemAnalyzer
{
    class DisckDetection // could use Singleton pattern in this case
    {

        public string path = @"C:\Temp\SystemAnalyzer.txt"; //it's no reason to make it public

        CultureInfo currentTime = new CultureInfo("en-GB");

        private static long start_Status_Memory { set; get; }
        private static long current_Status_Memory { set; get; }
        private static DriveInfo drive;

        public DisckDetection()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            drive = drives[0];
            ExistDirectory("C:\\Temp");
            start_Status_Memory = FreezeSize();
        }

        private static void ExistDirectory(string directoryName)// try not to mix static methods with class members
        {
            if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);
        }
        private static long FreezeSize()
        {
            return (ConvertToMB(drive.TotalFreeSpace));
        }

        private static long ConvertToMB(long b)
        {
            return (b / 1024) / 1024;
        }

        public void ChangeSizeMemory()
        {
            current_Status_Memory = FreezeSize();
            var tmpSize = start_Status_Memory - current_Status_Memory;
            if (tmpSize > 100.00 || tmpSize < -100.00)
            {
                this.WriteData(start_Status_Memory, tmpSize);
                start_Status_Memory = current_Status_Memory;
            }
        }
        // the difference between next 3 methods is only in message they write to file, maybe better solution will be create special service responsible for writing logs
        public void WriteData(long startSize, long tmpSize) // no reason to make it public
        {
            using (StreamWriter sr = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sr.WriteLine($@"[{String.Format(DateTime.Now.ToString(currentTime))}] --- Об'єм вiльного мiсця на диску змiнився iз {startSize}Мб на {current_Status_Memory}Мб з рiзницею {Math.Abs(tmpSize)}Мб.");
            }
        }

        public void WriteDataOnStart()
        {
            using (StreamWriter sr = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sr.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");
                sr.WriteLine($@"[{String.Format(DateTime.Now.ToString(currentTime))}] --- Об'єм вiльного мiсця на диску С на час запущення служби - {start_Status_Memory}Мб ");
            }
        }

        public void WriteDataOnEnd()
        {
            using (StreamWriter sr = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sr.WriteLine($@"[{String.Format(DateTime.Now.ToString(currentTime))}] --- Об'єм вiльного мiсця на диску С на час завершення роботи служби - {start_Status_Memory}Мб ");
            }
        }
    }
}
