using System;
using System.Collections.Generic;
using System.Text;


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
            NewQuiz newQuiz = new NewQuiz();
            _newQuiz = newQuiz;

            _newQuiz.Name = _enterName.Name;


            var window = new NewQuiz();
            window.ShowDialog();

        }

        public void SaveQuiz()
        {
            string Name = _newQuiz.Name;
            string Question = _newQuiz.Question;
            string FirstAnswer = _newQuiz.FirstAnswer;
            string SecondAnswer = _newQuiz.SecondAnswer;
            string ThirdAnswer = _newQuiz.ThirdAnswer;
            string FourthAnswer = _newQuiz.FourthAnswer;
            string CorrectlyAnswer = _newQuiz.CorrectlyAnswer;
            Model.CreateDataBase(Name, Question, FirstAnswer, SecondAnswer, ThirdAnswer, FourthAnswer, CorrectlyAnswer);
        }

        public void AddQuestions()
        {






        }
        public void DeleteQuestions()
        {

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
