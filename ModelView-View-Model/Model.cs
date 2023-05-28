using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;

namespace MVVM
{
    public class Model
    {
        
       
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
                string questionEncrypted = (string)reader["Question"];
                string firstAnswerEncrypted = (string)reader["FirstAnswer"];
                string secondAnswerEncrypted = (string)reader["SecondAnswer"];
                string thirdAnswerEncrypted = (string)reader["ThirdAnswer"];
                string fourthAnswerEncrypted = (string)reader["FourthAnswer"];
                string correctlyAnswerEncrypted = (string)reader["CorrectlyAnswer"];

                string question = DecryptData(questionEncrypted);
                string firstAnswer = DecryptData(firstAnswerEncrypted);
                string secondAnswer = DecryptData(secondAnswerEncrypted);
                string thirdAnswer = DecryptData(thirdAnswerEncrypted);
                string fourthAnswer = DecryptData(fourthAnswerEncrypted);
                string correctlyAnswer = DecryptData(correctlyAnswerEncrypted);

                string rowData = string.Join("| ", question, firstAnswer, secondAnswer, thirdAnswer, fourthAnswer, correctlyAnswer);
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
            


        }

        private static void InsertData(SQLiteConnection connection, string Name, string Question, string FirstAnswer, string SecondAnswer, string ThirdAnswer, string FourthAnswer, string CorrectlyAnswer)
        {
            string questionEncrypted = EncryptData(Question);
            string firstAnswerEncrypted = EncryptData(FirstAnswer);
            string secondAnswerEncrypted = EncryptData(SecondAnswer);
            string thirdAnswerEncrypted = EncryptData(ThirdAnswer);
            string fourthAnswerEncrypted = EncryptData(FourthAnswer);
            string correctlyAnswerEncrypted = EncryptData(CorrectlyAnswer);

            string query = $"INSERT INTO {Name} (Question, FirstAnswer, SecondAnswer, ThirdAnswer, FourthAnswer, CorrectlyAnswer) VALUES (@Question, @FirstAnswer, @SecondAnswer, @ThirdAnswer, @FourthAnswer, @CorrectlyAnswer)";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@Question", questionEncrypted);
            command.Parameters.AddWithValue("@FirstAnswer", firstAnswerEncrypted);
            command.Parameters.AddWithValue("@SecondAnswer", secondAnswerEncrypted);
            command.Parameters.AddWithValue("@ThirdAnswer", thirdAnswerEncrypted);
            command.Parameters.AddWithValue("@FourthAnswer", fourthAnswerEncrypted);
            command.Parameters.AddWithValue("@CorrectlyAnswer", correctlyAnswerEncrypted);

            command.ExecuteNonQuery();

        }

        
        private static void CreateDataBase(SQLiteConnection connection, string Path, string Name, List<string> data, int count)
        {

            for (int i = 0; i < count; i++) {
                string[] Base = data[i].Split('|');
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

        private static string EncryptData(string data)
        {

            string password = Properties.Settings.Default.Password;
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] encryptedBytes;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);

                AesManaged aes = new AesManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Key = hashedBytes;

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                            cryptoStream.Write(dataBytes, 0, dataBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            encryptedBytes = memoryStream.ToArray();
                        }
                    }
                }

                byte[] resultBytes = new byte[aes.IV.Length + encryptedBytes.Length];
                Array.Copy(aes.IV, 0, resultBytes, 0, aes.IV.Length);
                Array.Copy(encryptedBytes, 0, resultBytes, aes.IV.Length, encryptedBytes.Length);

                return Convert.ToBase64String(resultBytes);
            }
        }


        private static string DecryptData(string encryptedData)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(Properties.Settings.Default.Password);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            byte[] iv = new byte[16];
            byte[] dataBytes;

            Array.Copy(encryptedBytes, 0, iv, 0, iv.Length);
            Array.Copy(encryptedBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length - iv.Length);
            Array.Resize(ref encryptedBytes, encryptedBytes.Length - iv.Length);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);

                using (AesManaged aes = new AesManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Key = hashedBytes;
                    aes.IV = iv;

                    using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(encryptedBytes))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                using (StreamReader reader = new StreamReader(cryptoStream, Encoding.UTF8))
                                {
                                    string decryptedData = reader.ReadToEnd();
                                    return decryptedData;
                                }
                            }
                        }
                    }
                }
            }
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




        //public static void InsertData(string Name, string Question, string FirstAnswer, string SecondAnswer, string ThirdAnswer, string FourthAnswer, string CorrectlyAnswer)
        //{

        //    SQLiteConnection connection = new SQLiteConnection($"{path};Version=3;");

        //    string name = Name;
        //    string question = Question;
        //    string firstAnswer = FirstAnswer;
        //    string secondAnswer = SecondAnswer;
        //    string thirdAnswer = ThirdAnswer;
        //    string fourthAnswer = FourthAnswer;
        //    string correctlyAnswer = CorrectlyAnswer;


        //    connection.Open();
        //    if (connection.State == System.Data.ConnectionState.Open)
        //    {
        //        InsertData(connection, name, question, firstAnswer, secondAnswer, thirdAnswer, fourthAnswer, CorrectlyAnswer);
        //        connection.Close();
        //    }
        //    else
        //    {
        //        File.WriteAllText("неудача.txt", "неудача");
        //    }
        //}
        
        


       
        



    }
}
