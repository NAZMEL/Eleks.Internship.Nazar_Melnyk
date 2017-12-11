using System.Net.NetworkInformation;

namespace SystemAnalyzer
{
    class StatusNetwork : Recorder
    {
        string server = "google.com";
        Ping ping = new Ping();
        bool currentStatus;

        public StatusNetwork()
        {
            currentStatus = TestNetStatus();
        }

        bool TestNetStatus()
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
            if (currentStatus == tmp) { return; }

            currentStatus = tmp;
            string netStatus;

            if (tmp) { netStatus = "підключено"; }
            else { netStatus = "відключено"; }

            RecordMessage($"Зміна стану Інтернет-з'єднання - {netStatus}.");
        }

        public void WriteStatusNetStartOrEnd()
        {
            string netStatus;

            if (TestNetStatus()) { netStatus = "підключений"; }
            else { netStatus = "не підключений"; }

            RecordMessage($"Даний комп'ютер {netStatus} до Інтернету.");
        }

    }
}
