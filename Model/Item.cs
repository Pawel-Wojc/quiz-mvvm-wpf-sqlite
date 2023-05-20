using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_resolver.Model
{
    public class Item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private int _id;
        public int Id
        {
            get => _id; 
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private string _name = "";
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }



        private string _answerA = "A";
        public string AnswerA
        {
            get { return _answerA; }
            set {
                if (_answerA != value)
                {
                    _answerA = value;
                    OnPropertyChanged(nameof(AnswerA));
                }
            }
        }
        public bool _answerA_is_correct { get; set; }

        private string _answerB = "B";
        public string AnswerB
        {
            get { return _answerB; }
            set {
                if (_answerB != value)
                {
                    _answerB = value;
                    OnPropertyChanged(nameof(AnswerB));
                }
            }
        }
        public bool _answerB_is_correct { get; set; }

        private string _answerC = "C";
        public string AnswerC
        {
            get { return _answerC; }
            set {
                if (_answerC != value)
                {
                    _answerC = value;
                    OnPropertyChanged(nameof(AnswerC));
                }
            }
        }
        public bool _answerC_is_correct { get; set; }

        private string _answerD = "D";
        public string AnswerD
        {
            get { return _answerD; }
            set {
                if (_answerD != value)
                {
                    _answerD = value;
                    OnPropertyChanged(nameof(AnswerD));
                }
            }
        }
        public bool _answerD_is_correct { get; set; }






        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
