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
        

        private static List<string> ReadData(SQLiteConnection connection, string Path)
        {
            SQLiteDataReader reader;
            SQLiteCommand command;

            

            command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {Path}";
            reader = command.ExecuteReader();

            List<string> data = new List<string>();

            while (reader.Read())
            {
                string question = (string)reader["Question"];
                string firstAnswer = (string)reader["FirstAnswer"];
                string secondAnswer = (string)reader["SecondAnswer"];
                string thirdAnswer = (string)reader["ThirdAnswer"];
                string fourthAnswer = (string)reader["FourthAnswer"];
                string correctlyAnswer = (string)reader["CorrectlyAnswer"];

                // Tworzenie łańcucha znaków z danymi oddzielonymi przecinkami
                string rowData = string.Join(", ", question, firstAnswer, secondAnswer, thirdAnswer, fourthAnswer, correctlyAnswer);

                // Dodawanie łańcucha znaków do listy danych
                data.Add(rowData);
            }

            return data;
        }

        public static List<string> ReadData(string path, string FileName)
        {
            string FullPath = $"Data Source={path}"+$"\\"+$"{FileName};";
            SQLiteConnection connection = new SQLiteConnection(FullPath);
            connection.Open();
            string FileNameWithourExtension = Path.GetFileNameWithoutExtension(FileName);
            List<string> result = ReadData(connection, FileNameWithourExtension);
            connection.Close();
            return result;
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return new List<string>();
            //}


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


        private static void CreateDataBase(SQLiteConnection connection, string Path, string Name, List<string> data, int count)
        {

            for (int i = 0; i < count; i++) {
                string[] Base = data[i].Split(',');
                string question = Base[0];
                string firstanswer = Base[1];
                string secondanswer = Base[2];
                string thirdanswer = Base[3];
                string fourthanswer = Base[4];
                string correctlyanswer = Base[5];
                string name = Name;

                string createTableQuery = $"CREATE TABLE IF NOT EXISTS {Name} (Question TEXT NOT NULL, FirstAnswer TEXT NOT NULL, SecondAnswer TEXT NOT NULL, ThirdAnswer TEXT NOT NULL, FourthAnswer TEXT NOT NULL, CorrectlyAnswer TEXT NOT NULL)";
                using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }

                InsertData(connection, name, question, firstanswer, secondanswer, thirdanswer, fourthanswer, correctlyanswer);
                
            }
            connection.Close();
        }

        public static void CreateDataBase(List<string> Odpowiedz, string Name, int count, string path)
        {
            //string path_for_base = $"C:/Users/hudze/source/repos/MVVM/ModelView-View-Model/";
            //string Path_for_CreateDataBase = $"C:/Users/hudze/source/repos/MVVM/ModelView-View-Model/{Name}.db";
            string Path = $"Data Source={path}";
            SQLiteConnection connection = new SQLiteConnection($"{Path};Version=3;");
            
            connection.Open();
            CreateDataBase(connection, path, Name, Odpowiedz, count);
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
        
        private static void DropTable(SQLiteConnection connection, List<string> Odpowiedz, string Name, int count, string path)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"DROP TABLE {Name}";
            command.ExecuteNonQuery();
            CreateDataBase(connection, path, Name, Odpowiedz, count);


        }

        public static void DropTable(List<string> Odpowiedz, string Name, int count, string path)
        {
            SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Version=3;");

            connection.Open();
            DropTable(connection, Odpowiedz, Name, count, path);
            connection.Close();
        }


       
        



    }
}
