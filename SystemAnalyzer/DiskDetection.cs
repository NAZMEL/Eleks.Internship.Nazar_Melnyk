using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;

namespace SystemAnalyzer
{
    class DisckDetection
    {

        public string path = @"C:\Temp\SystemAnalyzer.txt";

        CultureInfo currentTime = new CultureInfo("en-GB");

        private static long start_Status_Memory { set; get; }
        private static long current_Status_Memory { set; get; }
        private static DriveInfo drive;

        public DisckDetection()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            drive = drives[0];
            start_Status_Memory = FreezeSize();
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

        public void WriteData(long startSize, long tmpSize)
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
