using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace MVVM
{
    public class NewQuizViewModel
    {
        private NewQuiz _newQuiz;
        private EnterName _enterName;
        
        private Model _model;

        public NewQuizViewModel(NewQuiz newQuiz)
        {
            _newQuiz = newQuiz;



            _newQuiz.DeleteQuestions += _newQuiz_DeleteQuestions;
            _newQuiz.AddQuestions += _newQuiz_AddQuestions;
            _newQuiz.SaveQuiz += _newQuiz_SaveQuiz;
        }

        public NewQuizViewModel(EnterName enterName) {
            _enterName = enterName;

            _enterName.ContinueCreateQuiz += _enterName_ContinueCreateQuiz;
            _enterName.CancelCreateQuiz += _enterName_CancelCreateQuiz;

        }
        


        private void _enterName_CancelCreateQuiz()
        {
            CancelQuiz();
        }

        private void _enterName_ContinueCreateQuiz()
        {
            ContinueCreateQuiz();
        }

        private void _newQuiz_SaveQuiz()
        {
            SaveQuiz();
        }

        private void _newQuiz_AddQuestions()
        {
            AddQuestions();
        }

        private void _newQuiz_DeleteQuestions()
        {
            DeleteQuestions();
        }

        

        public void ContinueCreateQuiz()
        {

            NewQuiz newquiz = new NewQuiz();
            newquiz.NameTextBoxNewQuiz.Text = _enterName.Name;

            if (newquiz.NameTextBoxNewQuiz.Text == "")
            {
                MessageBox.Show("Proszę wpisać nazwę Quizu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (MainWindowModelView.enterNameWindow != null)
            {
                MainWindowModelView.enterNameWindow.Close();
            }

            


            newquiz.ShowDialog();
           


        }

        public void SaveQuiz()
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Data Base (*.db)|*.db|All Files (*.*)|*.*";
            saveFileDialog.InitialDirectory = @"C:\";
            saveFileDialog.Title = "Save File";
            saveFileDialog.FileName = _newQuiz.NameTextBoxNewQuiz.Text;

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedFilePath = saveFileDialog.FileName;
                string SelectedFileName = Path.GetFileNameWithoutExtension(selectedFilePath);
                int count = _newQuiz.ListBoxNewQuiz.Items.Count;

                List<string> pytania = new List<string>();
                if (count != 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        string formatteddata = _newQuiz.ListBoxNewQuiz.Items[i].ToString();
                        string cleanedText = formatteddata.Replace("Question: ", "")
                            .Replace("FirstAnswer: ", "")
                            .Replace("SecondAnswer: ", "")
                            .Replace("ThirdAnswer: ", "")
                            .Replace("FourthAnswer: ", "")
                            .Replace("CorrectlyAnswer: ", "");

                        pytania.Add(cleanedText);
                    }

                    

                    // Удаляем существующий файл, если он существует
                    if (File.Exists(selectedFilePath))
                    {
                        File.Delete(selectedFilePath);
                    }

                    // Создаем новый файл базы данных
                    Model.CreateDataBase(pytania, SelectedFileName, count, selectedFilePath);

                    MessageBox.Show("Quiz został utworzony!", "Powiodło się!", MessageBoxButton.OK, MessageBoxImage.Information);
                    _newQuiz.Close();
                }
                else
                {
                    MessageBox.Show("Dla zapisania musi być przynajmniej jeden element!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //int count = _newQuiz.ListBoxNewQuiz.Items.Count;

                //List<string> pytania = new List<string>();


                //if (count != 0)
                //{
                //    for(int i = 0; i < count; i++)
                //    {
                //        string formatteddata = _newQuiz.ListBoxNewQuiz.Items[i].ToString();
                //        string cleanedText = formatteddata.Replace("Question: ", "")
                //            .Replace("FirstAnswer: ", "")
                //            .Replace("SecondAnswer: ", "")
                //            .Replace("ThirdAnswer: ", "")
                //            .Replace("FourthAnswer: ", "")
                //            .Replace("CorrectlyAnswer: ", "");


                //        pytania.Add(cleanedText);
                //    }
                //    String Name = _newQuiz.NameTextBoxNewQuiz.Text;
                //    //Model.CreateDataBase(pytania, Name, count); ИСПРАВИТЬ!!!!!!!!!!!!!
                //    MessageBox.Show("Quiz został utworzony!", "Powiodło się!", MessageBoxButton.OK, MessageBoxImage.Information);
                //    _newQuiz.Close();
                //}
                //else
                //{
                //    MessageBox.Show("Dla zapisania musi być przynajmniej jeden element!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
                // Выводит ошибку, что нет элементов 


            }
                //Model.CreateDataBase(Name, Question, FirstAnswer, SecondAnswer, ThirdAnswer, FourthAnswer, CorrectlyAnswer);
        }

        private bool IsAnyTextBoxEmpty()
        {
            if (_newQuiz.FirstAnswerTextBoxNewQuiz.Text == "" || _newQuiz.SecondAnswerTextBoxNewQuiz.Text == "" || _newQuiz.ThirdAnswerTextBoxNewQuiz.Text == "" || _newQuiz.FourthAnswerTextBoxNewQuiz.Text == "")
            {
                return true;
            }
            return false;
        }



        private bool isAnyRadioButtonChecked()
        {
            if (_newQuiz.FirstAnswerRadioButton.IsChecked == true || _newQuiz.SecondAnswerRadioButton.IsChecked == true || _newQuiz.ThirdAnswerRadioButton.IsChecked == true || _newQuiz.FourthAnswerRadioButton.IsChecked == true)
            {
                
                return true;
            }

            return false;
        }


        public void AddQuestions()
        {
            if(!isAnyRadioButtonChecked() || IsAnyTextBoxEmpty())
            {
                MessageBox.Show("Proszę sprawdzić poprawność danych. Czy wszystkie dane są wprowadzone?", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }






            string Name = _newQuiz.Name;
            string Question = _newQuiz.Question;
            string FirstAnswer = _newQuiz.FirstAnswer;
            string SecondAnswer = _newQuiz.SecondAnswer;
            string ThirdAnswer = _newQuiz.ThirdAnswer;
            string FourthAnswer = _newQuiz.FourthAnswer;
            string CorrectlyAnswer = _newQuiz.CorrectlyAnswer;
            string DaneDoListBox = string.Join($", ", Question, FirstAnswer, SecondAnswer, ThirdAnswer, FourthAnswer, CorrectlyAnswer);
            string formattedString = string.Format("Question: {0}| FirstAnswer: {1}| SecondAnswer: {2}| ThirdAnswer: {3}| FourthAnswer: {4}| CorrectlyAnswer: {5}", Question, FirstAnswer, SecondAnswer, ThirdAnswer, FourthAnswer, CorrectlyAnswer);
            _newQuiz.ListBoxNewQuiz.Items.Add(formattedString);
            _newQuiz.ClearForm();



        }
        public void DeleteQuestions()
        {
            if(_newQuiz.ListBoxIndex != -1)
            {
                int Index = _newQuiz.ListBoxIndex;
                _newQuiz.ListBoxNewQuiz.Items.RemoveAt(Index);
            }
            else
            {
                MessageBox.Show("Nie ma elementów dla usuwania", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            // else
            // Показывает ошибку, что нет элементов в списке для удаления
        }

        
        public void CancelQuiz()
        {
            if(MainWindowModelView.enterNameWindow != null)
            {
                MainWindowModelView.enterNameWindow.Close();
            }
            

        }

        





    }
}
