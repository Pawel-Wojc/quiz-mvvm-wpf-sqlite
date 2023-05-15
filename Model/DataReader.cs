using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;

namespace quiz_resolver.Model
{
    internal class DataReader
    {
     

        private SQLiteConnection sqlite_conn;

        

        public DataReader() {
            sqlite_conn = CreateConnection();


            System.Diagnostics.Debug.WriteLine("Konstruktor data reader");
           


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
                System.Diagnostics.Debug.WriteLine("Polaczenie ok");
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

            ObservableCollection<Item> quiz_table = new ObservableCollection< Item>();
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                
                Item item = new Item();
                item.Id = sqlite_datareader.GetInt16(0);
                item.Name = sqlite_datareader.GetString(1);
                // quiz_table.Add(quiz_id, quiz_name);
                quiz_table.Add( item);
                

            }
            
            return quiz_table;
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
                item.Name = sqlite_datareader.GetString(2);
                // quiz_table.Add(quiz_id, quiz_name);

                // odpowiedzi odarazu
                SQLiteDataReader sqlite_datareader_answer;
                SQLiteCommand sqlite_cmd_answer;
                sqlite_cmd_answer = sqlite_conn.CreateCommand();
                sqlite_cmd_answer.CommandText = "SELECT * FROM answer where question_id = " + item.Id;
                sqlite_datareader_answer = sqlite_cmd_answer.ExecuteReader();
                int i = 0;
                while (sqlite_datareader_answer.Read()) {
                    if (i == 0) item.AnswerA = sqlite_datareader_answer.GetString(2);
                    else if (i == 1) item.AnswerB = sqlite_datareader_answer.GetString(2);
                    else if (i == 2) item.AnswerC = sqlite_datareader_answer.GetString(2);
                    else if (i == 3) item.AnswerD = sqlite_datareader_answer.GetString(2);
                    i++;
                }

              question_table.Add(item);

                string test = sqlite_datareader.GetInt16(1).ToString() + " " + sqlite_datareader.GetString(2) + " A: " + item.AnswerA + "B " + item.AnswerB + "C " + item.AnswerC + "D " + item.AnswerD;
                System.Diagnostics.Debug.WriteLine(test);
            }
            
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
                    if (i == 0)
                    {
                        selecetedQuestion.AnswerA = sqlite_datareader_answer.GetString(2);

                        // tymczasowo sprawdzam czy jest 1 czy 0, pozniej bedzie base64

                        if (sqlite_datareader_answer.GetInt16(3) == 1) {
                            selecetedQuestion._answerA_is_correct = true;
                        }
                        
                    }
                    else if (i == 1) { 
                        selecetedQuestion.AnswerB = sqlite_datareader_answer.GetString(2);
                        if (sqlite_datareader_answer.GetInt16(3) == 1)
                        {
                            selecetedQuestion._answerB_is_correct = true;
                        }
                    }
                    else if (i == 2) {
                        selecetedQuestion.AnswerC = sqlite_datareader_answer.GetString(2);
                        if (sqlite_datareader_answer.GetInt16(3) == 1)
                        {
                            selecetedQuestion._answerC_is_correct = true;
                        }
                    }
                    else if (i == 3) {
                        selecetedQuestion.AnswerD = sqlite_datareader_answer.GetString(2);
                        if (sqlite_datareader_answer.GetInt16(3) == 1)
                        {
                            selecetedQuestion._answerD_is_correct = true;
                        }

                    }             
                i++;
                }
            }
            return ListQuestion;
         
        }


        ~DataReader()
        {
            sqlite_conn.Close();
        }



    }
}
