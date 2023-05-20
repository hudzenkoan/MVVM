using System;
using System.Collections.Generic;
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
            if (MainWindowModelView.enterNameWindow != null)
            {
                MainWindowModelView.enterNameWindow.Close();
            }

            newquiz.ShowDialog();
           


        }

        public void SaveQuiz()
        {

            //string Name = _newQuiz.Name;
            //string Question = _newQuiz.Question;
            //string FirstAnswer = _newQuiz.FirstAnswer;
            //string SecondAnswer = _newQuiz.SecondAnswer;
            //string ThirdAnswer = _newQuiz.ThirdAnswer;
            //string FourthAnswer = _newQuiz.FourthAnswer;
            //string CorrectlyAnswer = _newQuiz.CorrectlyAnswer;
            int count = _newQuiz.ListBoxNewQuiz.Items.Count;

            List<string> pytania = new List<string>();
            

            if (count != 0)
            {
                for(int i = 0; i < count; i++)
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
                String Name = _newQuiz.NameTextBoxNewQuiz.Text;
                Model.CreateDataBase(pytania, Name, count);
                
            }
            else
            {
                MessageBox.Show("Dla zapisania musi być przynajmniej jeden element!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            // Выводит ошибку, что нет элементов 



            //Model.CreateDataBase(Name, Question, FirstAnswer, SecondAnswer, ThirdAnswer, FourthAnswer, CorrectlyAnswer);
        }

        public void AddQuestions()
        {
            string Name = _newQuiz.Name;
            string Question = _newQuiz.Question;
            string FirstAnswer = _newQuiz.FirstAnswer;
            string SecondAnswer = _newQuiz.SecondAnswer;
            string ThirdAnswer = _newQuiz.ThirdAnswer;
            string FourthAnswer = _newQuiz.FourthAnswer;
            string CorrectlyAnswer = _newQuiz.CorrectlyAnswer;
            string DaneDoListBox = string.Join($", ", Question, FirstAnswer, SecondAnswer, ThirdAnswer, FourthAnswer, CorrectlyAnswer);
            string formattedString = string.Format("Question: {0}, FirstAnswer: {1}, SecondAnswer: {2}, ThirdAnswer: {3}, FourthAnswer: {4}, CorrectlyAnswer: {5}", Question, FirstAnswer, SecondAnswer, ThirdAnswer, FourthAnswer, CorrectlyAnswer);
            _newQuiz.ListBoxNewQuiz.Items.Add(formattedString);



        }
        public void DeleteQuestions()
        {
            if(_newQuiz.ListBoxIndex != -1)
            {
                int Index = _newQuiz.ListBoxIndex;
                _newQuiz.ListBoxNewQuiz.Items.RemoveAt(Index);
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
