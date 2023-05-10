using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.ComponentModel;

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


        public ObservableCollection<Item> ReadQuestions(Item selecetedQuiz)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM question where quiz_id = "+selecetedQuiz.Id.ToString();

            ObservableCollection<Item> question_table = new ObservableCollection<Item>();
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                Item item = new Item();
                item.Id = sqlite_datareader.GetInt16(0);
                item.Name = sqlite_datareader.GetString(2);
                // quiz_table.Add(quiz_id, quiz_name);
                question_table.Add(item);
                System.Diagnostics.Debug.WriteLine(sqlite_datareader.GetInt16(1).ToString()+" " + sqlite_datareader.GetString(2));
            }
            
            return question_table;
        }
        ~DataReader()
        {
            sqlite_conn.Close();
        }



    }
}
