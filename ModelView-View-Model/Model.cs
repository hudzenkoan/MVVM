using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace MVVM
{
    public class Model
    {
        //zmienne
        static string path = "Data Source=C:/Users/hudze/source/repos/MVVM/ModelView-View-Model/Quiz.db";
        public string Find { get; }
        public string Question { get; set; }
        public string FirstAnswer{get; set;}
        public string SecondAnswer { get; set; }
        public string ThirdAnswer { get; set; }
        public string FourthAnswer { get; set; }
        public string NameQuiz { get; set; }
        

        private static void ReadData(SQLiteConnection connection)
        {
            SQLiteDataReader reader;
            SQLiteCommand command;

            command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Quiz";
            reader = command.ExecuteReader();

            while (reader.Read()) {
                string Name = (string)reader["NameQuiz"];
                string Que = (string)reader["Question"];
                string First = (string)reader["FirstAnswer"];
                string Second = (string)reader["SecondAnswer"];
                string Third = (string)reader["ThirdAnswer"];
                string Fourth = (string)reader["FourthAnswer"];
                string Correctly = (string)reader["CorrectlyAnswer"];
            }
        }

        public static void ReadData()
        {
            SQLiteConnection connection = new SQLiteConnection($"{path};Version=3;");
            try
            {
                connection.Open();
                ReadData(connection);
                connection.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void InsertData(SQLiteConnection connection, string Name, string Question, string FirstAnswer, string SecondAnswer, string ThirdAnswer, string FourthAnswer, string CorrectlyAnswer)
        {
            string query = $"INSERT INTO {Name} (Question, FirstAnswer, SecondAnswer, ThirdAnswer, FourthAnswer, CorrectlyAnswer) VALUES (@Question, @FirstAnswer, @SecondAnswer, @ThirdAnswer, @FourthAnswer, @CorrectlyAnswer)";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@Question", Question); 
            command.Parameters.AddWithValue("@FirstAnswer", FirstAnswer); 
            command.Parameters.AddWithValue("@SecondAnswer", SecondAnswer); 
            command.Parameters.AddWithValue("@ThirdAnswer", ThirdAnswer); 
            command.Parameters.AddWithValue("@FourthAnswer", FourthAnswer);
            command.Parameters.AddWithValue("@CorrectlyAnswer", CorrectlyAnswer);

            command.ExecuteNonQuery();

        }


        private static void CreateDataBase(SQLiteConnection connection, string Path, string Name, string Question, string FirstAnswer, string SecondAnswer, string ThirdAnswer, string FourthAnswer, string CorrectlyAnswer)
        {
            
            string createTableQuery = $"CREATE TABLE IF NOT EXISTS {Name} (Question TEXT NOT NULL, FirstAnswer TEXT NOT NULL, SecondAnswer TEXT NOT NULL, ThirdAnswer TEXT NOT NULL, FourthAnswer TEXT NOT NULL, CorrectlyAnswer TEXT NOT NULL)";
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, connection))
            {
                createTableCommand.ExecuteNonQuery();
            }

            InsertData(connection, Name, Question, FirstAnswer, SecondAnswer, ThirdAnswer, FourthAnswer, CorrectlyAnswer);
            connection.Close();

        }

        public static void CreateDataBase(string Name, string Question, string FirstAnswer, string SecondAnswer, string ThirdAnswer, string FourthAnswer, string CorrectlyAnswer)
        {
            string path_for_base = $"C:/Users/hudze/source/repos/MVVM/ModelView-View-Model/";
            string Path_for_CreateDataBase = $"C:/Users/hudze/source/repos/MVVM/ModelView-View-Model/{Name}.db";
            string path = $"Data Source={path_for_base}{Name}.db";
            SQLiteConnection connection = new SQLiteConnection($"{path};Version=3;");
            
            connection.Open();
            CreateDataBase(connection, Path_for_CreateDataBase, Name, Question, FirstAnswer, SecondAnswer, ThirdAnswer, FourthAnswer, CorrectlyAnswer);
            connection.Close();
         
            

        }




        public static void InsertData(string Name, string Question, string FirstAnswer, string SecondAnswer, string ThirdAnswer, string FourthAnswer, string CorrectlyAnswer)
        {

            SQLiteConnection connection = new SQLiteConnection($"{path};Version=3;");

            string name = Name;
            string question = Question;
            string firstAnswer = FirstAnswer;
            string secondAnswer = SecondAnswer;
            string thirdAnswer = ThirdAnswer;
            string fourthAnswer = FourthAnswer;
            string correctlyAnswer = CorrectlyAnswer;


            connection.Open();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                InsertData(connection, name, question, firstAnswer, secondAnswer, thirdAnswer, fourthAnswer, CorrectlyAnswer);
                connection.Close();
            }
            else
            {
                File.WriteAllText("неудача.txt", "неудача");
            }
        }
        



        //zdarzenia
        event Action NewQuiz;
        event Action EditQuiz;
        event Action OpenQuiz;

       //metody

        public void NowyQuiz()
        {

        }
        



    }
}
