using quiz_resolver.Model;
using quiz_resolver.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        public string question_number { get; set; }

        private Stopwatch _stopwatch;
        private TimeSpan _elapsedTime;
        private bool _isRunning;
        private int Points = 0;
        private int maxPoints = 0;
        private bool _is_button_one_selected;
        public bool is_button_one_selected {
            get { return _is_button_one_selected; }
            set
            {
                if (_is_button_one_selected != value)
                {
                    _is_button_one_selected = value;
                    OnPropertyChanged(nameof(is_button_one_selected));
                }
            }
        }
        private bool _is_button_two_selected;
        public bool is_button_two_selected
        {
            get { return _is_button_two_selected; }
            set
            {
                if (_is_button_two_selected != value)
                {
                    _is_button_two_selected = value;
                    OnPropertyChanged(nameof(is_button_two_selected));
                }
            }
        }
        private bool _is_button_three_selected;
        public bool is_button_three_selected
        {
            get { return _is_button_three_selected; }
            set
            {
                if (_is_button_three_selected != value)
                {
                    _is_button_three_selected = value;
                    OnPropertyChanged(nameof(is_button_three_selected));
                }
            }
        }
        private bool _is_button_four_selected;
        public bool is_button_four_selected
        {
            get { return _is_button_four_selected; }
            set
            {
                if (_is_button_four_selected != value)
                {
                    _is_button_four_selected = value;
                    OnPropertyChanged(nameof(is_button_four_selected));
                }
            }
        }
        public ICommand StartCommand { get; set; }
        public ICommand NextQuestionCommand { get; set; }

        public Item currentQuestion { get; set; }
        private string _answer_a;
        public string answer_a {
            get { return _answer_a; }
            set
            {
                if (_answer_a != value)
                {
                    _answer_a = value;
                    OnPropertyChanged(nameof(answer_a));
                }
            }
        }
        private string _answer_b;
        public string answer_b {
            get { return _answer_b; }
            set
            {
                if (_answer_b != value)
                {
                    _answer_b = value;
                    OnPropertyChanged(nameof(answer_b));
                }
            }
        }
        private string _answer_c;
        public string answer_c {
            get { return _answer_c; }
            set
            {
                if (_answer_c != value)
                {
                    _answer_c = value;
                    OnPropertyChanged(nameof(answer_c));
                }
            }
        }
        private string _answer_d;
        public string answer_d {
            get { return _answer_d; }
            set
            {
                if (_answer_d != value)
                {
                    _answer_d = value;
                    OnPropertyChanged(nameof(answer_d));
                }
            }
        }
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
        

        private string _question_Property_Content;
        public string Question_Property_Content {
            get { return _question_Property_Content; }
            set
            {
                if (_question_Property_Content != value)
                {
                    _question_Property_Content = value;
                    OnPropertyChanged(nameof(Question_Property_Content));
                }
            }
        }
        //question table
        private List<Item> question_table;
        public List<Item> Question_table
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
        private Item selectedQuiz;
        public Item SelectedQuiz
        {
            get { return selectedQuiz; }
            set
            {
                if (selectedQuiz != value)
                {
                    selectedQuiz = value;
                    OnPropertyChanged(nameof(SelectedQuiz));
                }
            }
        }




        DataReader _Database;

        
        public MainViewModel()
        {
            _Database = new DataReader();
            
            
            
            _stopwatch = new Stopwatch();

            ReadQuizes();


            
            
            NextQuestionCommand = new RelayCommand(NexQuestionButtonClicker, CanNextQuestion);
            StartCommand = new RelayCommand(StartButtonClicked, CanStart);
            
            //PropertyChanged += TimerViewModel_PropertyChanged;   PO CO?
        }

        private void NexQuestionButtonClicker(object obj)
        {
            ShowNextQuestion();
            Show_Current_Answers();
            Debug.WriteLine("Klikenito next");
                     
        }

        //
        public void ReadQuizes() {
            Quiz_table = new ObservableCollection<Item>();
            Quiz_table = _Database.ReadQuizzes();
        }

        
        public void StartButtonClicked(object obj)
        {
            StartTimer(obj);
            
            ShowNextQuestion();
            Show_Current_Answers();
        }

        private void Show_Current_Answers()
        {      
            answer_a = currentQuestion.AnswerA;
            answer_b = currentQuestion.AnswerB;
            answer_c = currentQuestion.AnswerC;
            answer_d = currentQuestion.AnswerD;

            is_button_one_selected = is_button_two_selected = is_button_three_selected = is_button_four_selected = false;
            
        }


        public void ShowNextQuestion() {
            int current_index = 1;


            if (currentQuestion == null)
            {
                Question_table = question_from_db();
                maxPoints = Question_table.Count;
                currentQuestion = Question_table.First();
                Question_Property_Content = currentQuestion.Name;
                
                
                
            }
            else if (Question_table.Count() - 1 != Question_table.IndexOf(currentQuestion))
            {
                current_index = Question_table.IndexOf(currentQuestion) ;
                currentQuestion = Question_table.ElementAt(current_index + 1);
                Question_Property_Content = currentQuestion.Name;
                current_index += 2; //2 because iteration starts from 0 and we change question to next one

            }
            else {
                current_index = maxPoints;
                end_of_questions();

            }
            question_number = (current_index).ToString()+" of "+maxPoints.ToString();
            OnPropertyChanged(nameof(question_number));


        }

        private void end_of_questions()
        {
            Debug.WriteLine("Koniec pytan");
        }

       
        public List<Item> question_from_db() {         
            List<Item> tabelka = new();
            tabelka = _Database.ReadQuestions(SelectedQuiz);
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
            if (selectedQuiz != null && !_isRunning)
            {
                // _Database.ReadQuestions(SelectedItem);

                return true;

            }
            else
            {
                return false;
            }
        }


        public bool CanNextQuestion(object obj)
        {
            if (is_button_one_selected || is_button_two_selected || is_button_three_selected || is_button_four_selected) return true;
            else return false;
            
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
