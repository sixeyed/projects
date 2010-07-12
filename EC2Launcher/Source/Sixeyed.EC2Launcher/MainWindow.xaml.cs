using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Sixeyed.EC2Launcher.Configuration;
using Sixeyed.EC2Launcher.Providers;
using Sixeyed.EC2Launcher.Structs;

namespace Sixeyed.EC2Launcher
{
    /// <summary>
    /// Main launcher window
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private instance fields

        private bool _launchedRdp;
        private int _elapsedMinutes;
        private DispatcherTimer _stopTimer;
        private BackgroundWorker _worker = new BackgroundWorker();

        #endregion

        #region Private instance properties

        private string CurrentStateName { get; set; }
        private string ExpectedStateName { get; set; }
        private string PublicDnsName { get; set; }
        private string InstanceId
        {
            get{ return AwsConfiguration.InstanceId;}
        }

        #endregion

        #region BackgroundWorker Event handlers

        void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UpdateUIStatus();
        }

        void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateUIStatus();
        }

        void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            GetCurrentState();
            _worker.ReportProgress(0);
            while (ExpectedStateName != null && CurrentStateName != ExpectedStateName)
            {
                Thread.Sleep(AwsConfiguration.UpdateWaitMilliseconds);
                GetCurrentState();
            }
        }

        #endregion 

        #region UI Event handlers

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var encryptor = new Encryptor();
            if (encryptor.ShowDialog().Value)
            {
                EC2.Provider.AccessKey = AwsConfiguration.AccessKey;
                EC2.Provider.SecretKey = AwsConfiguration.SecretKey;
                SetStatus(null);
                menu1.Visibility = Visibility.Hidden;
                Title = AwsConfiguration.PCName;
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            _launchedRdp = false;
            chkAutoStop.IsEnabled = false;
            var instance = EC2.StartInstance(InstanceId);
            CurrentStateName = instance.CurrentStateName;
            StartTimer();
            SetStatus(EC2InstanceState.Running);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            StopInstance();
        }

        void _stopTimer_Tick(object sender, EventArgs e)
        {
            _elapsedMinutes++;
            if (CurrentStateName == EC2InstanceState.Running)
            {
                lblTimer.Content = string.Format("... for {0} minutes", _elapsedMinutes);
            }
            else
            {
                lblTimer.Content = string.Empty;
            }
            if (chkAutoStop.IsChecked.Value && _elapsedMinutes >= AwsConfiguration.StopTimeoutMinutes)
            {
                StopInstance();
            }
        }

        #endregion

        #region Private instance methods

        public MainWindow()
        {
            InitializeComponent();
            InitializeUI();
            if (EC2.IsConfigured())
            {
                SetStatus(null);
                menu1.Visibility = Visibility.Hidden;
            }
        }

        private void InitializeUI()
        {
            Title = AwsConfiguration.PCName;
            chkAutoStop.Content = string.Format("Stop the PC after {0} minutes if I forget", AwsConfiguration.StopTimeoutMinutes);
            _worker.WorkerReportsProgress = true;
            _worker.DoWork += new DoWorkEventHandler(_worker_DoWork);
            _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_worker_RunWorkerCompleted);
            _worker.ProgressChanged += new ProgressChangedEventHandler(_worker_ProgressChanged);            
        }

        private void UpdateUIStatus()
        {
            lblTimer.Content = string.Empty;
            if (CurrentStateName == null)
            {
                MessageBox.Show(string.Format("Instance ID: {0} not found from Service Url: {1}", InstanceId, AwsConfiguration.ServiceUrl), "Instance Not Found");
                btnStart.IsEnabled = btnStop.IsEnabled = false;
                lblState.Content = "PC not found";
            }
            else
            {
                lblState.Content = string.Format("Status: {0}", CurrentStateName);
                btnStart.IsEnabled = (CurrentStateName == EC2InstanceState.Stopped);
                btnStop.IsEnabled = (CurrentStateName == EC2InstanceState.Running);
            }
            if (CurrentStateName == EC2InstanceState.Running)
            {
                StartTimer();
                btnLaunchRdp.IsEnabled = CurrentStateName == EC2InstanceState.Running;
                LaunchRdp();
            }
        }  

        private void SetStatus(string expectedStateName)
        {
            ExpectedStateName = expectedStateName;
            if (!_worker.IsBusy)
            {
                _worker.RunWorkerAsync();
            }
        }

        private void GetCurrentState()
        {
            var instance = EC2.GetInstance(InstanceId);
            CurrentStateName = instance.CurrentStateName;
            if (instance.IsRunning())
            {
                PublicDnsName = instance.PublicDnsName;               
            }            
        }

        private void StopInstance()
        {
            StopTimer();
            chkAutoStop.IsEnabled = true;
            var instance = EC2.StopInstance(InstanceId);
            CurrentStateName = instance.CurrentStateName;
            SetStatus(EC2InstanceState.Stopped);
        }

        private void LaunchRdp(bool force=false)
        {
            if (force || !_launchedRdp)
            {
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = AwsConfiguration.RdpPath;
                startInfo.Arguments = string.Format(AwsConfiguration.RdpDnsArgumentFormat, PublicDnsName);
                var process = Process.Start(startInfo);
                _launchedRdp = true;
            }
        }

        private void StartTimer()
        {
            _elapsedMinutes = 0;
            _stopTimer = new DispatcherTimer();
            _stopTimer.Interval = TimeSpan.FromMinutes(1);
            _stopTimer.Tick += new EventHandler(_stopTimer_Tick);
            _stopTimer.IsEnabled = true;
            _stopTimer.Start();
        }

        private void StopTimer()
        {
            _stopTimer.Stop();
            _elapsedMinutes = 0;
            _stopTimer.IsEnabled = false;            
            _stopTimer = null;
        }

        #endregion

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            LaunchRdp(true);
        }
    }
}
