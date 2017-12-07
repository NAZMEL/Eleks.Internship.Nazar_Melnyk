using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Net.NetworkInformation;
using System.Globalization;
using System.IO;

namespace SystemAnalyzer
{
    delegate void WriteMessage();
    public partial class SysService : ServiceBase
    {

        private System.Timers.Timer timer;

        event WriteMessage Recording;
        DisckDetection watcher;
        StatusNetwork watcherNet;

        public SysService()
        {
            InitializeComponent();

            this.ServiceName = "SysService";
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
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timerElapsed);
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
            watcher.WriteDataOnEnd();
            watcherNet.WriteStatusNetStartOrEnd();
        }

        private void timerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Recording();
        }

    }
}

