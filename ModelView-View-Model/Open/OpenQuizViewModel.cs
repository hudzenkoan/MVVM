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
        


        public OpenQuizViewModel()
        {
            
        }
        public void Open()
        {
            OpenWindow();
        }

        private void OpenWindow()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bazy danych (*.db)|*.db|Wszystkie pliki (*.*)|*.*";
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Title = "Wybierz plik bazy danych";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                SelectedFilePath = openFileDialog.FileName;
                SelectedFileName = Path.GetFileNameWithoutExtension(SelectedFilePath);

                // Читаем данные и сохраняем их в QuizData
                QuizData = Model.ReadData(SelectedFilePath, SelectedFileName);

                // Открываем диалоговое окно с передачей QuizData в конструктор
                OpenQuiz window = new OpenQuiz();
                _openQuiz = window;
                _openQuiz.QuizData = QuizData;

                // Создаем экземпляр OpenQuizViewModel и передаем ссылку на окно OpenQuiz
                _openQuiz.DataContext = this;

                // Подписываемся на события окна OpenQuiz
                _openQuiz.SubmitAnswer += SubmitAnswerHandler;
                _openQuiz.CheckAnswer += CheckAnswerHandler;

                // Переходим к первому вопросу
                MoveToNextQuestion();

                // Открываем диалоговое окно
                _openQuiz.ShowDialog();
            }
        }
        private void ShowResultsWindow()
        {
            ResultsWindow resultsWindow = new ResultsWindow();
            // Przekaż wyniki do okna wyników (możesz dodać parametry do konstruktora ResultsWindow)
            // ...
            resultsWindow.ShowDialog();
        }

        private void MoveToNextQuestion()
        {
            List<string> Data = _openQuiz.QuizData;
            // Проверяем, что есть еще вопросы в QuizData
            if (count >= Data.Count)
            {
                ShowResultsWindow();
                // Zamknij bieżące okno gry
                _openQuiz.Close();
                return;
            }
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
        

        

        public void SubmitAnswerHandler()
        {
            string selectedAnswer = _openQuiz.Answer;

            // Проверяем ответ и выполняем соответствующие действия
            if (selectedAnswer == CorrectlyAnswer)
            {
                // Правильный ответ
                MessageBox.Show("Odpowiedź prawidłowa!");

                // Переходим к следующему вопросу
                MoveToNextQuestion();
            }
            else
            {
                // Неправильный ответ
                MessageBox.Show("Odpowiedź nieprawidłowa!");

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