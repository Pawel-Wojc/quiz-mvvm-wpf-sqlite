using quiz_resolver.ViewModel;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;

namespace quiz_resolver.ViewModels
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        public delegate void get_time(String currentTime);
        
        public event PropertyChangedEventHandler? PropertyChanged;
        private Stopwatch stopWatch;
        public TimerViewModel()
        {
            stopWatch = new Stopwatch();

        }
        public void TimerStart (bool run){
            
            stopWatch.Start();
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += Timer_Tick;
            timer.Start();
        }

        public void Timer_Tick(object sender, EventArgs e, get_time obj)
        {
            //_elapsedTime = _stopwatch.Elapsed;
            //OnPropertyChanged("ElapsedTime");
            TimeSpan _elapsedTime = stopWatch.Elapsed;
            string time = _elapsedTime.ToString();
            obj(time);

        }



        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
