using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;


namespace MVVM
{
    public class EditQuestionsViewModel
    {
        private EditQuestions _editQuestions;

        public EditQuestionsViewModel(EditQuestions editQuestions)
        {
            _editQuestions = editQuestions;

            _editQuestions.AddQuestion += _editQuestions_AddQuestion;
            _editQuestions.SaveQuiz += _editQuestions_SaveQuiz;
            _editQuestions.DeleteQuestions += _editQuestions_DeleteQuestions;
        }

        private void _editQuestions_DeleteQuestions()
        {
            DeleteQuestions();
        }

        private void _editQuestions_SaveQuiz()
        {
            SaveQuiz();
        }

        private void _editQuestions_AddQuestion()
        {
            AddQuestion();
        }

        public void DeleteQuestions()
        {


            if (_editQuestions.ListBoxIndex != -1)
            {
                int Index = _editQuestions.ListBoxIndex;
                _editQuestions.ListBox_EditQuestions.Items.RemoveAt(Index);
            }
            else
            {
                MessageBox.Show("Nie ma elementów dla usuwania", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        public void SaveQuiz()
        {

        }
        public void AddQuestion()
        {
            string Name = _editQuestions.Name;
            string Question = _editQuestions.Question;
            string FirstAnswer = _editQuestions.FirstAnswer;
            string SecondAnswer = _editQuestions.SecondAnswer;
            string ThirdAnswer = _editQuestions.ThirdAnswer;
            string FourthAnswer = _editQuestions.FourthAnswer;
            string CorrectlyAnswer = _editQuestions.CorrectlyAnswer;
            string DaneDoListBox = string.Join($", ", Question, FirstAnswer, SecondAnswer, ThirdAnswer, FourthAnswer, CorrectlyAnswer);
            string formattedString = string.Format("Question: {0}, FirstAnswer: {1}, SecondAnswer: {2}, ThirdAnswer: {3}, FourthAnswer: {4}, CorrectlyAnswer: {5}", Question, FirstAnswer, SecondAnswer, ThirdAnswer, FourthAnswer, CorrectlyAnswer);
            _editQuestions.ListBox_EditQuestions.Items.Add(formattedString);
        }
    }
}
