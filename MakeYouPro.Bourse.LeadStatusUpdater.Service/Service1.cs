using System;
using System.Net;
using System.ServiceProcess;
using System.Timers;


namespace MakeYouPro.Bourse.LeadStatusUpdater.Service
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer();

            timer.Interval = TimeSpan.FromHours(24).TotalMilliseconds;
            DateTime nowTime = DateTime.Now;
            DateTime scheduledTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 2, 0, 0);

            if (scheduledTime < nowTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            TimeSpan timeUntilScheduled = scheduledTime - nowTime;
            timer.Elapsed += TimerElapsed;

            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
            timer.Dispose();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            string url = "http://example.com/get-data";
            using (WebClient client = new WebClient())
            {
                string response = client.DownloadString(url);
            }
        }
    }
}
