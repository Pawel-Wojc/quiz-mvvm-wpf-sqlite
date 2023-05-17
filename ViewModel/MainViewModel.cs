﻿using quiz_resolver.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;


namespace quiz_resolver.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string question_number { get; set; }
        private bool _end_of_questions = true;
        private Stopwatch _stopwatch;
        private TimeSpan _elapsedTime;
        private bool _isRunning;
        private int _earned_points = 0;
        public string earned_points { get; set; }
        private int _maxPoints = 0;
        private bool _is_button_one_selected;

        #region Properties
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
            //PropertyChanged += TimerViewModel_PropertyChanged;   PO CO?
        }

        private void NexQuestionButtonClicker(object obj)
        {
            CheckAsnwers();
            ShowNextQuestion();
            Show_Current_Answers();
            Debug.WriteLine("Klikenito next");
                     
        }

        //
        public void ReadQuizes() {
            _database = new DataReader();
            quiz_table = new ObservableCollection<Item>();
            quiz_table = _database.ReadQuizzes();
        }

        
        public void StartButtonClicked(object obj)
        {
            _earned_points = 0;
            earned_points = "Points: 0";
            OnPropertyChanged(nameof(earned_points));

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
                _end_of_questions = false;
                question_table = QuestionFromDB();
                _maxPoints = question_table.Count;
                currentQuestion = question_table.FirstOrDefault();
                Question_Property_Content = currentQuestion.Name;
             
            }
            else if (question_table.Count() - 1 != question_table.IndexOf(currentQuestion))
            {
                current_index = question_table.IndexOf(currentQuestion) ;
                currentQuestion = question_table.ElementAt(current_index + 1);
                Question_Property_Content = currentQuestion.Name;
                current_index += 2; //2 because iteration starts from 0 and we change question to next one

            }
            else {
                current_index = _maxPoints;
                EndOfQuestions();

            }
            question_number = (current_index).ToString()+" of "+_maxPoints.ToString();
            OnPropertyChanged(nameof(question_number));


        }

        public void CheckAsnwers() {
            if (is_button_one_selected == currentQuestion._answerA_is_correct && is_button_two_selected == currentQuestion._answerB_is_correct && is_button_three_selected == currentQuestion._answerC_is_correct && is_button_four_selected == currentQuestion._answerD_is_correct) {
                _earned_points += 1;
                earned_points = "Points: " + _earned_points.ToString();
                OnPropertyChanged(nameof(earned_points));
            }
            
        }

        private void EndOfQuestions()
        {
            _end_of_questions = true;
            Question_Property_Content = "Result:" + earned_points.ToString() + " of " + _maxPoints.ToString() + " in " + _elapsedTime.ToString();
            
        }

       
        public List<Item> QuestionFromDB() {         
            List<Item> tabelka = new();
            tabelka = _database.ReadQuestions(selected_quiz);
            Debug.WriteLine("przed foreach");
            Debug.WriteLine(tabelka.Count());
            foreach (Item item in tabelka)
            { 
                Debug.WriteLine(item.Name);
            }
            Debug.WriteLine("po foreach");
            if (tabelka == null) Debug.WriteLine("tabelka to null");
            return tabelka;
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
            if (_selected_quiz != null && !_isRunning)
            {
                
                return true;

            }
            else
            {
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
