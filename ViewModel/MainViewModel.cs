using quiz_resolver.Model;
using quiz_resolver.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace quiz_resolver.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        private bool _end_of_questions = true;
        private Stopwatch _stopwatch;
        private TimeSpan _elapsedTime;
        private bool _isRunning;
        private int _earned_points = 0;
        private int _maxPoints = 0;
        
        public DispatcherTimer timer = new DispatcherTimer();


        #region Properties
        public Item? currentQuestion { get; set; }
        private string _elapsed_time = "";
        public string elapsed_time
        {
            get { return _elapsed_time; }
            set
            {
                if (_elapsed_time != value)
                {
                    _elapsed_time = value;
                    OnPropertyChanged(nameof(elapsed_time));
                }
            }
        }

        private string _points = "";
        public string points
        {
            get { return _points; }
            set
            {
                if (_points != value)
                {
                    _points = value;
                    OnPropertyChanged(nameof(points));
                }
            }
        }

        private string _question_number = "";
        public string question_number
        {
            get { return _question_number; }
            set
            {
                if (_question_number != value)
                {
                    _question_number = value;
                    OnPropertyChanged(nameof(question_number));
                }
            }
        }
        private bool _is_button_one_selected;
        public bool is_button_one_selected 
        {
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
        
        
        private string _answer_a = "";
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
        private string _answer_b = "";
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
        private string _answer_c = "";
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
        private string _answer_d = "";
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
        private ObservableCollection<Item> _quiz_table;
        public ObservableCollection<Item> quiz_table
        {
            get { return _quiz_table; }
            set
            {
                if (_quiz_table != value)
                {
                    _quiz_table = value;
                    OnPropertyChanged(nameof(quiz_table));
                }
            }
        }
        

        private string _question_Property_Content = "";
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
        private List<Item> _question_table;
        public List<Item> question_table
        {
            get { return _question_table; }
            set
            {
                if (_question_table != value)
                {
                    _question_table = value;
                    OnPropertyChanged(nameof(question_table));
                }
            }
        }
        private Item _selected_quiz;

        public Item selected_quiz
        {
            get { return _selected_quiz; }
            set
            {
                if (_selected_quiz != value)
                {
                    _selected_quiz = value;
                    OnPropertyChanged(nameof(selected_quiz));
                }
            }
        }

        #endregion


        DataReader _database;

        public ICommand start_command { get; set; }
        public ICommand next_question_command { get; set; }

        public MainViewModel()
        {
            
            ReadQuizes();            
            _stopwatch = new Stopwatch();
            next_question_command = new RelayCommand(NexQuestionButtonClicker, CanNextQuestion);
            start_command = new RelayCommand(StartButtonClicked, CanStart);
     
        }

        private void NexQuestionButtonClicker(object obj)
        {
            CheckAsnwers();
            ShowNextQuestion();                    
        }

        //
        public void ReadQuizes() {
            _database = new DataReader();
            quiz_table = new ObservableCollection<Item>();
            quiz_table = _database.ReadQuizzes();
        }

        
        public void StartButtonClicked(object obj)
        {
            _end_of_questions = false;
            _earned_points = 0;
            points = "Points: 0";
            elapsed_time = "00:00:00";

            StartTimer(obj);
            ShowNextQuestion();
           
            
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
                _end_of_questions = false;
                question_table = _database.ReadQuestions(selected_quiz);
                _maxPoints = question_table.Count;
                currentQuestion = question_table.FirstOrDefault();
                Question_Property_Content = currentQuestion.Name;
                Show_Current_Answers();

            }
            else if (question_table.Count() - 1 != question_table.IndexOf(currentQuestion))
            {
                current_index = question_table.IndexOf(currentQuestion) ;
                currentQuestion = question_table.ElementAt(current_index + 1);
                Question_Property_Content = currentQuestion.Name;
                current_index += 2; //2 because iteration starts from 0 and we change question to next one
                Show_Current_Answers();
                question_number = (current_index).ToString() + " of " + _maxPoints.ToString();

            }
            else {
                current_index = _maxPoints;
                Show_Current_Answers();
                EndOfQuestions();

            }
            
           
            
        }

        public void CheckAsnwers() {
            if (is_button_one_selected == currentQuestion._answerA_is_correct && is_button_two_selected == currentQuestion._answerB_is_correct && is_button_three_selected == currentQuestion._answerC_is_correct && is_button_four_selected == currentQuestion._answerD_is_correct) {
                _earned_points += 1;
                points = "Points: " + _earned_points.ToString();
                
            }
            
        }

        private void EndOfQuestions()
        {
            StopTimer();
            
            string message = points.ToString() + " of " + _maxPoints.ToString() + "\nTime: " + elapsed_time.ToString();
            MessageBox.Show(message);
            
            _end_of_questions = true;
            selected_quiz = null;
            currentQuestion = null;
            question_number = "";
            points = "";
            elapsed_time = "";
            answer_a = answer_b = answer_c = answer_d = "";
            Question_Property_Content = "";



            OnPropertyChanged(nameof(elapsed_time));
        }

       
        public List<Item> QuestionFromDB() {         
            return _database.ReadQuestions(selected_quiz);
        }

        public bool CanNextQuestion(object obj)
        {
            if (!_end_of_questions)
            {
                if (is_button_one_selected || is_button_two_selected || is_button_three_selected || is_button_four_selected) return true;
                else return false;
            }
            else return false;
        }



        public bool CanStart(object obj)
        {
            if (selected_quiz != null && !_isRunning)
            {

                return true;

            }
            else
            {
                return false;
            }
        }



        //TIMER
        public void StartTimer(object obj)
        {
            _isRunning = true;
            _stopwatch.Reset();
            _stopwatch.Start();

            // Powiadomienie o zmianie w czasie co 100 milisekund
            
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void StopTimer() {
            _isRunning = false;
            timer.Stop();
        }


        public void Timer_Tick(object sender, EventArgs e)
        {
            elapsed_time = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
            OnPropertyChanged(nameof(elapsed_time));
        }

        // OnPropertyChanged
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }     
    }
}
