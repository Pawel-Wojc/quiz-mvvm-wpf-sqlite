using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Collections.ObjectModel;

namespace quiz_resolver.Model
{
    internal class DataReader
    {
        private SQLiteConnection sqlite_conn;      

        public DataReader() {
            sqlite_conn = CreateConnection();
            System.Diagnostics.Debug.WriteLine("Constructor data reader");
        }

        static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            string db_source = "D:\\GDrive pawelitwojcik\\Studia\\Semestr 4\\Programowanie obiektowe i graficzne\\quiz_resolver\\quiz_resolver\\database.db";
            sqlite_conn = new SQLiteConnection("Data Source="+db_source+"; Version = 3; New = True; Compress = True; ");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
                System.Diagnostics.Debug.WriteLine("Connecting good");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }
            return sqlite_conn;
        }

        public ObservableCollection<Item> ReadQuizzes()
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM quiz";

            ObservableCollection<Item> _quiz_table = new ObservableCollection< Item>();
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {            
                Item item = new Item();
                item.Id = sqlite_datareader.GetInt16(0);
                item.Name = sqlite_datareader.GetString(1);
                // _quiz_table.Add(quiz_id, quiz_name);
                _quiz_table.Add( item);               
            }           
            return _quiz_table;
        }

        public List<Item> ReadQuestions(Item selecetedQuiz)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM question where quiz_id = "+selecetedQuiz.Id.ToString();

            List<Item> question_table = new List<Item>();
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                Item item = new Item();
                item.Id = sqlite_datareader.GetInt16(0);
                item.Name = Base64Decode(sqlite_datareader.GetString(2));
                question_table.Add(item);
                //string test = sqlite_datareader.GetInt16(1).ToString() + " " + sqlite_datareader.GetString(2) + " A: " + item.AnswerA + "B " + item.AnswerB + "C " + item.AnswerC + "D " + item.AnswerD;
                //System.Diagnostics.Debug.WriteLine(test);              
            }
            System.Diagnostics.Debug.WriteLine("odczytałem pytania");
            return ReadAnswers(question_table);
        }

        public List<Item> ReadAnswers(List<Item> ListQuestion) // 
        {
            foreach (Item selecetedQuestion in ListQuestion) {
                SQLiteDataReader sqlite_datareader;
                SQLiteDataReader sqlite_datareader_answer;
                SQLiteCommand sqlite_cmd_answer;
                sqlite_cmd_answer = sqlite_conn.CreateCommand();
                sqlite_cmd_answer.CommandText = "SELECT * FROM answer where question_id = " + selecetedQuestion.Id;
                sqlite_datareader_answer = sqlite_cmd_answer.ExecuteReader();
                List<Item> answers = new List<Item>();
                int i = 0;
                while (sqlite_datareader_answer.Read())
                {
                    uint _answer_correctness = UInt32.Parse(Base64Decode(sqlite_datareader_answer.GetString(3))) % 2; //correct answers are saved id db as a random even numbers, max 4,294,967,295
                    if (i == 0)
                    {
                        selecetedQuestion.AnswerA = Base64Decode(sqlite_datareader_answer.GetString(2));

                        if (_answer_correctness == 0) {
                            selecetedQuestion._answerA_is_correct = true;
                        }
                        
                    }
                    else if (i == 1) { 
                        selecetedQuestion.AnswerB = Base64Decode(sqlite_datareader_answer.GetString(2));
                        if (_answer_correctness == 0)
                        {
                            selecetedQuestion._answerB_is_correct = true;
                        }
                    }
                    else if (i == 2) {
                        selecetedQuestion.AnswerC = Base64Decode(sqlite_datareader_answer.GetString(2));
                        if (_answer_correctness == 0)
                        {
                            selecetedQuestion._answerC_is_correct = true;
                        }
                    }
                    else if (i == 3) {
                        selecetedQuestion.AnswerD = Base64Decode(sqlite_datareader_answer.GetString(2));
                        if (_answer_correctness == 0)
                        {
                            selecetedQuestion._answerD_is_correct = true;
                        }

                    }             
                i++;
                }
            }
            //System.Diagnostics.Debug.WriteLine("odczytałem odpowiedzi");
            return ListQuestion;       
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        ~DataReader()
        {
            sqlite_conn.Close();
        }
    }
}
