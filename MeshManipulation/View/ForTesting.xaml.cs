using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeshManipulation.View
{
    /// <summary>
    /// Interaction logic for ForTesting.xaml
    /// </summary>
    public partial class ForTesting : UserControl
    {
        public ForTesting()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;

            TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();
            
            Task glownyTask = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                Dispatcher.Invoke(() => { pbStatus.Value = 10; });
                Thread.Sleep(2000);
                Dispatcher.Invoke(() => { pbStatus.Value = 30; });
                Thread.Sleep(2000);
                Dispatcher.Invoke(() => { pbStatus.Value = 60; });
                Thread.Sleep(2000);
                Dispatcher.Invoke(() => { pbStatus.Value = 90; });
                Thread.Sleep(2000);
                Dispatcher.Invoke(() => { pbStatus.Value = 100; });
            });

            Duration duration = new Duration(TimeSpan.FromSeconds(30));
            DoubleAnimation animation = new DoubleAnimation(200, duration);

            //Task pbTask = new Task(() => {
            //    pbStatus.BeginAnimation(ProgressBar.ValueProperty, animation);
            //});

            //pbTask.RunSynchronously();

            glownyTask
            .ContinueWith((task) =>
            {
                btnStart.IsEnabled = true;
            }, uiThread);

        }

        private void btnStart_Click1(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += (senderWorker, doWorkEA) => {
                for (int i = 0; i < 100; ++i)
                {
                    (senderWorker as BackgroundWorker).ReportProgress(i);
                    Thread.Sleep(100);
                }
            };

            worker.ProgressChanged += (senderWorker, progressChangedEA) => {
                pbStatus.Value = progressChangedEA.ProgressPercentage;
            };

            worker.RunWorkerAsync();

        }
    }
}
