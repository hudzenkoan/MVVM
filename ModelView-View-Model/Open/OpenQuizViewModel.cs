﻿using Microsoft.Win32;
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
        private static int IloscPrawidlowychOdpowiedzi;
        private static int IloscPytan;
        private static List<string> OdpowiedziUzytkownika;


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
                
                QuizData = Model.ReadData(folderpath, SelectedFileName);

                
                OpenQuiz window = new OpenQuiz();
                _openQuiz = window;
                _openQuiz.QuizData = QuizData;

                
                _openQuiz.DataContext = this;

                
                _openQuiz.SubmitAnswer += SubmitAnswerHandler;
                
                _openQuiz.Rozpocznij_Click += _openQuiz_Rozpocznij_Click;
                _openQuiz.Zakoncz_Click += _openQuiz_Zakoncz_Click;


                IloscPytan = QuizData.Count;


                
                _openQuiz.ShowDialog();

                
            }
        }

        private void _openQuiz_Zakoncz_Click()
        {
            


           if(_openQuiz!= null)
            {
                _openQuiz.timer.Stop();
                _openQuiz.Close();
            }
            
            

            _resultsWindow = new ResultsWindow(this);
            

            string Wynik = $"Wynik: {IloscPrawidlowychOdpowiedzi}/{IloscPytan}";



            

            _resultsWindow.Wynik = Wynik;
            _resultsWindow.ZapiszWyniki += _resultsWindow_ZapiszWyniki;
            _resultsWindow.NieZapisujWynikow += _resultsWindow_NieZapisujWynikow;

            _resultsWindow.ShowDialog();

            



        }

        private void _resultsWindow_NieZapisujWynikow()
        {
            _resultsWindow.Close();
        }

        private void _resultsWindow_ZapiszWyniki()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Notepad (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.InitialDirectory = @"C:\";
            saveFileDialog.Title = "Save File";

            bool? result = saveFileDialog.ShowDialog();

            if(result == true)
            {
                string selectedFilePath = saveFileDialog.FileName;
                

                TextWriter txt = new StreamWriter(selectedFilePath);

                foreach(string files in OdpowiedziUzytkownika)
                {
                    txt.Write(files+"\n");
                    
                }
                txt.Close();
                MessageBox.Show("Plik został stworzony", "Powiodło się", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            
        }

        public void Zakoncz_metod()
        {
            _openQuiz_Zakoncz_Click();
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
            
            MoveToNextQuestion();
            if (!_openQuiz.timerStarted)
            {
                _openQuiz.timer.Start();
                _openQuiz.timerStarted = true;
            }
        }

        

        private void MoveToNextQuestion()
        {
            List<string> Data = _openQuiz.QuizData;
            
            if (count < Data.Count)
            {
                string rowData = QuizData[count];
                string[] rowDataParts = rowData.Split('|');

                Question = rowDataParts[0].Trim();
                FirstAnswer = rowDataParts[1].Trim();
                SecondAnswer = rowDataParts[2].Trim();
                ThirdAnswer = rowDataParts[3].Trim();
                FourthAnswer = rowDataParts[4].Trim();
                CorrectlyAnswer = rowDataParts[5].Trim();

                
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

                _openQuiz.FirstAnswerRadioButtonOpenQuiz.IsChecked = false;
                _openQuiz.SecondAnswerRadioButtonOpenQuiz.IsChecked = false;
                _openQuiz.ThirdAnswerRadioButtonOpenQuiz.IsChecked = false;
                _openQuiz.FourthAnswerRadioButtonOpenQuiz.IsChecked = false;
                return;
            }
            
        }


        private bool IsAnyRadioButtonChecked()
        {
            if (_openQuiz.FirstAnswerRadioButtonOpenQuiz.IsChecked == true || _openQuiz.SecondAnswerRadioButtonOpenQuiz.IsChecked == true ||
                _openQuiz.ThirdAnswerRadioButtonOpenQuiz.IsChecked == true || _openQuiz.FourthAnswerRadioButtonOpenQuiz.IsChecked == true)
            {
                return true;
            }

            return false;
        }

        public void SubmitAnswerHandler()
        {
            if(OdpowiedziUzytkownika == null)
            {
                OdpowiedziUzytkownika = new List<string>();
            }

            string Question = _openQuiz.Question;
            string selectedAnswer = _openQuiz.Answer;

            OdpowiedziUzytkownika.Add($"Question: {Question}, Twoja Odpowiedź: {selectedAnswer}, Poprawna Odpowiedź: {CorrectlyAnswer} ");


            if (!IsAnyRadioButtonChecked())
            {
                MessageBox.Show("Proszę wybrać odpowiedź", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            
            
            if (selectedAnswer == CorrectlyAnswer)
            {
                
                IloscPrawidlowychOdpowiedzi++;
                
                MoveToNextQuestion();
            }
            else
            {
                
                MoveToNextQuestion();
            }

            _openQuiz.FirstAnswerRadioButtonOpenQuiz.IsChecked = false;
            _openQuiz.SecondAnswerRadioButtonOpenQuiz.IsChecked = false;
            _openQuiz.ThirdAnswerRadioButtonOpenQuiz.IsChecked = false;
            _openQuiz.FourthAnswerRadioButtonOpenQuiz.IsChecked = false;


        }
        

    }
}