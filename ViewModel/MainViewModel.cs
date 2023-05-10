using quiz_resolver.Model;
using quiz_resolver.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace quiz_resolver.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private Stopwatch _stopwatch;
        private TimeSpan _elapsedTime;
        private bool _isRunning;
        public ICommand StartCommand { get; set; }
        

        //quiz table
        private ObservableCollection<Item> quiz_table;
        public ObservableCollection<Item> Quiz_table
        {
            get { return quiz_table; }
            set
            {
                if (quiz_table != value)
                {
                    quiz_table = value;
                    OnPropertyChanged(nameof(quiz_table));
                }
            }
        }

        private string _question;
        public string Question {
            get { return _question; }
            set
            {
                if (_question != value)
                {
                    _question = value;
                    OnPropertyChanged(nameof(Question));
                }
            }
        }
        //question table
        private ObservableCollection<Item> question_table;
        public ObservableCollection<Item> Question_table
        {
            get { return question_table; }
            set
            {
                if (question_table != value)
                {
                    question_table = value;
                    OnPropertyChanged(nameof(Question_table));
                }
            }
        }




        DataReader _Database;

        
        public MainViewModel()
        {
            _Database = new DataReader();
            Quiz_table = new ObservableCollection<Item>();
            
            Quiz_table = _Database.ReadQuizzes();        
            _stopwatch = new Stopwatch();


            StartCommand = new RelayCommand(StartButtonClicked, CanStart);
            
            //PropertyChanged += TimerViewModel_PropertyChanged;   PO CO?
        }

        //


        public void StartButtonClicked(object obj)
        {
            StartTimer(obj);
            przycisk_start(obj);

        }


        private Item selectedItem;
        public Item SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }



        public void przycisk_start(object o) {
            Question_table = question_from_db();
            Question = Question_table.First().Name;
            OnPropertyChanged("Button_Start");
            

        }
        public ObservableCollection<Item> question_from_db() {         
            ObservableCollection<Item> tabelka = new();
            tabelka = _Database.ReadQuestions(SelectedItem);
            return tabelka;
        }

 
        // TIMER


        public string ElapsedTime
        {
            get
            {
                return "Time: " + _elapsedTime.ToString(@"hh\:mm\:ss");
            }
        }

        public bool CanStart(object obj)
        {
            if (selectedItem != null && !_isRunning)
            {
                // _Database.ReadQuestions(SelectedItem);
                
                return true;

            }
            else {
                return false;
            }
        }

        public void StartTimer(object obj)
        {
            
            _isRunning = true;
            _stopwatch.Start();

            // Powiadomienie o zmianie w czasie co 100 milisekund
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            _elapsedTime = _stopwatch.Elapsed;
            OnPropertyChanged("ElapsedTime");
        }

        public void TimerViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ElapsedTime" && !_isRunning)
            {
                _stopwatch.Reset();
            }
        }


        // OnPropertyChanged
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}
