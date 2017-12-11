using System;
using System.ServiceProcess;

namespace SystemAnalyzer
{
    public partial class SysService : ServiceBase
    {
        private System.Timers.Timer timer;

        Action Recording;

        DisckDetection watcher;
        StatusNetwork watcherNet;

        public SysService()
        {
            InitializeComponent();

            this.CanStop = true;
            this.CanPauseAndContinue = false;
            this.AutoLog = true;

            watcher = new DisckDetection();
            watcherNet = new StatusNetwork();

            Recording += watcher.ChangeSizeMemory;
            Recording += watcherNet.WriteStatusNet;
        }

        protected override void OnStart(string[] args)
        {
            this.timer = new System.Timers.Timer();
            this.timer.Enabled = true;
            this.timer.AutoReset = true;
            this.timer.Interval = 10000;
            this.timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) => Recording();
            this.timer.Start();

            watcher.WriteDataOnStart();
            watcherNet.WriteStatusNetStartOrEnd();
        }

        protected override void OnStop()
        {
            timer.Stop();
            watcher.WriteDataOnEnd();
            watcherNet.WriteStatusNetStartOrEnd();
        }

        protected override void OnShutdown()
        {
            timer.Stop();
            watcher.WriteDataOnEnd();
            watcherNet.WriteStatusNetStartOrEnd();
        }
    }
}

