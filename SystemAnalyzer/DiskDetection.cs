using System;
using System.IO;

namespace SystemAnalyzer
{
    class DisckDetection : Recorder
    {
        static long start_Status_Memory { set; get; }
        static long current_Status_Memory { set; get; }
        DriveInfo drive;

        public DisckDetection()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            drive = drives[0];
            ExistDirectory("C:\\Temp");
            start_Status_Memory = FreezeSize();
        }

        void ExistDirectory(string directoryName)
        {
            if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);
        }

        long FreezeSize()
        {
            return (ConvertToMB(drive.TotalFreeSpace));
        }

        long ConvertToMB(long b)
        {
            return (b / 1024) / 1024;
        }

        public void ChangeSizeMemory()
        {
            current_Status_Memory = FreezeSize();
            var tmpSize = start_Status_Memory - current_Status_Memory;

            if (tmpSize > 100.00 || tmpSize < -100.00)
            {
                RecordMessage($"Об'єм вiльного мiсця на диску C змiнився iз {start_Status_Memory}Мб на {current_Status_Memory}Мб з рiзницею {Math.Abs(tmpSize)}Мб.");

                start_Status_Memory = current_Status_Memory;
            }
        }

        public void WriteDataOnStart()
        {
            RecordMessage("-----------------------------------------------------------------------------------------------------------------------------");
            RecordMessage($"Об'єм вiльного мiсця на диску С на час запущення служби - {start_Status_Memory} Мб.");
        }

        public void WriteDataOnEnd()
        {
            RecordMessage($"Об'єм вiльного мiсця на диску С на час завершення роботи служби - {start_Status_Memory} Мб.");
        }
    }
}
