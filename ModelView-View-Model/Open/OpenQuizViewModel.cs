using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

using System.Windows.Media;
using System.Windows.Threading;

namespace MVVM
{
    public class OpenQuizViewModel
    {
        private OpenQuiz _openQuiz;
        private MainWindowModelView _mainWindowModelView;
        private ResultsWindow _resultsWindow;
        private static string SelectedFilePath { get; set; }
        private static string SelectedFileName { get; set; }
        private string Question { get; set; }
        private string FirstAnswer { get; set; }
        private string SecondAnswer { get; set; }
        private string ThirdAnswer { get; set; }
        private string FourthAnswer { get; set; }
        private string CorrectlyAnswer { get; set; }
        private int count { get; set; }
        private List<string> QuizData { get; set; }
        private int IloscPrawidlowychOdpowiedzi;
        private int IloscPytan;


        public OpenQuizViewModel()
        {

        }
        public void Open()
        {
            OpenWindow();
        }

        public void OpenWindowDoubleClick(string Path, string FileName)
        {
            QuizData = Model.ReadData(Path, FileName);

            OpenQuiz window = new OpenQuiz();
            _openQuiz = window;
            _openQuiz.QuizData = QuizData;

            _openQuiz.DataContext = this;

            _openQuiz.SubmitAnswer += SubmitAnswerHandler;
            _openQuiz.CheckAnswer += CheckAnswerHandler;
            _openQuiz.Rozpocznij_Click += _openQuiz_Rozpocznij_Click;
            _openQuiz.Zakoncz_Click += _openQuiz_Zakoncz_Click;

            IloscPytan = QuizData.Count;

            _openQuiz.ShowDialog();
        }

        private void OpenWindow()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bazy danych (*.db)|*.db|Wszystkie pliki (*.*)|*.*";
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Title = "Wybierz plik bazy danych";
            string folderpath;

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                SelectedFilePath = openFileDialog.FileName;
                
                SelectedFileName = Path.GetFileNameWithoutExtension(SelectedFilePath) + ".db";

                folderpath = Path.GetDirectoryName(SelectedFilePath);
                // Читаем данные и сохраняем их в QuizData
                QuizData = Model.ReadData(folderpath, SelectedFileName);

                // Открываем диалоговое окно с передачей QuizData в конструктор
                OpenQuiz window = new OpenQuiz();
                _openQuiz = window;
                _openQuiz.QuizData = QuizData;

                // Создаем экземпляр OpenQuizViewModel и передаем ссылку на окно OpenQuiz
                _openQuiz.DataContext = this;

                // Подписываемся на события окна OpenQuiz
                _openQuiz.SubmitAnswer += SubmitAnswerHandler;
                _openQuiz.CheckAnswer += CheckAnswerHandler;
                _openQuiz.Rozpocznij_Click += _openQuiz_Rozpocznij_Click;
                _openQuiz.Zakoncz_Click += _openQuiz_Zakoncz_Click;


                IloscPytan = QuizData.Count;


                // Открываем диалоговое окно
                _openQuiz.ShowDialog();
            }
        }

        private void _openQuiz_Zakoncz_Click()
        {
            _openQuiz.timer.Stop();

            _resultsWindow = new ResultsWindow(this);
            //_resultsWindow.DataContext = this;

            string Wynik = $"Wynik: {IloscPrawidlowychOdpowiedzi}/{IloscPytan}";

            _resultsWindow.Wynik = Wynik;
            _resultsWindow.ShowDialog();

            // Закрыть текущее окно игры
            _openQuiz.Close();

        }


        private void _openQuiz_Rozpocznij_Click()
        {

            _openQuiz.FirstAnswerRadioButtonOpenQuiz.IsChecked = false;
            _openQuiz.SecondAnswerRadioButtonOpenQuiz.IsChecked = false;
            _openQuiz.ThirdAnswerRadioButtonOpenQuiz.IsChecked = false;
            _openQuiz.FourthAnswerRadioButtonOpenQuiz.IsChecked = false;
            IloscPrawidlowychOdpowiedzi = 0;
            _openQuiz.Rozpocznij_ustawienie = false;
            _openQuiz.Submit_ustawienie = true;
            _openQuiz.Check_Ustawienia = true;
            MoveToNextQuestion();
            if (!_openQuiz.timerStarted)
            {
                _openQuiz.timer.Start();
                _openQuiz.timerStarted = true;
            }
        }

        //private void ShowResultsWindow()
        //{
        //    ResultsWindow resultsWindow = new ResultsWindow(this);

        //    resultsWindow.ShowDialog();
        //}

        private void MoveToNextQuestion()
        {
            List<string> Data = _openQuiz.QuizData;
            // Проверяем, что есть еще вопросы в QuizData
            if (count < Data.Count)
            {
                string rowData = QuizData[count];
                string[] rowDataParts = rowData.Split(',');

                Question = rowDataParts[0].Trim();
                FirstAnswer = rowDataParts[1].Trim();
                SecondAnswer = rowDataParts[2].Trim();
                ThirdAnswer = rowDataParts[3].Trim();
                FourthAnswer = rowDataParts[4].Trim();
                CorrectlyAnswer = rowDataParts[5].Trim();

                // Обновляем данные в окне
                _openQuiz.Question = Question;
                _openQuiz.FirstAnswer = FirstAnswer;
                _openQuiz.SecondAnswer = SecondAnswer;
                _openQuiz.ThirdAnswer = ThirdAnswer;
                _openQuiz.FourthAnswer = FourthAnswer;

                count++;

            }
            else {
                _openQuiz.SubmitButtonOpenQuiz.IsEnabled = false;
                _openQuiz.FirstAnswerRadioButtonOpenQuiz.Content = "";
                _openQuiz.SecondAnswerRadioButtonOpenQuiz.Content = "";
                _openQuiz.ThirdAnswerRadioButtonOpenQuiz.Content = "";
                _openQuiz.FourthAnswerRadioButtonOpenQuiz.Content = "";
                _openQuiz.QuestionLabelOpenQuiz.Content = "Pytania się skonczyły. Kliknij Zakoncz dla wyświetlenia wyników ";


                return;
            }
            
        }




        public void SubmitAnswerHandler()
        {
            string selectedAnswer = _openQuiz.Answer;

            // Проверяем ответ и выполняем соответствующие действия
            if (selectedAnswer == CorrectlyAnswer)
            {
                // Правильный ответ
                //MessageBox.Show("Odpowiedź prawidłowa!");
                IloscPrawidlowychOdpowiedzi++;
                // Переходим к следующему вопросу
                MoveToNextQuestion();
            }
            else
            {
                // Неправильный ответ
                //MessageBox.Show("Odpowiedź nieprawidłowa!");

                // Переходим к следующему вопросу
                MoveToNextQuestion();
            }
        }

        public void CheckAnswerHandler()
        {
            // Проверяем правильный ответ и отображаем сообщение
            MessageBox.Show($"Poprawna odpowiedź: {CorrectlyAnswer}");
        }

    }
}