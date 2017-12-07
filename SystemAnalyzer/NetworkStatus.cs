using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Globalization;
using System.IO;

namespace SystemAnalyzer
{
    class StatusNetwork
    {
        string server = "google.com";
        Ping ping = new System.Net.NetworkInformation.Ping();
        public string path = @"C:\Temp\SystemAnalyzer.txt";
        CultureInfo currentTime = new CultureInfo("en-GB");
        bool general;
        public StatusNetwork()
        {
            general = TestNetStatus();
        }

        public bool TestNetStatus()
        {
            try
            {
                ping.Send(server);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void WriteStatusNet()
        {
            bool tmp = TestNetStatus();
            if (general == tmp) { return; }
            general = tmp;
            string netStatus;
            if (tmp) { netStatus = "підключено"; }
            else { netStatus = "відключено"; }

            using (StreamWriter sr = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sr.WriteLine($@"[{String.Format(DateTime.Now.ToString(currentTime))}] --- Зміна стану Інтернет-з'єднання - {netStatus}.");
            }
        }

        public void WriteStatusNetStartOrEnd()
        {
            string netStatus;
            if (TestNetStatus()) { netStatus = @"Даний комп'ютер підключений до Інтернету"; }
            else { netStatus = @"Даний комп'ютер не підключений до Інтернету"; }

            using (StreamWriter sr = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sr.WriteLine($@"[{String.Format(DateTime.Now.ToString(currentTime))}] --- {netStatus}.");
            }
        }

    }
}
