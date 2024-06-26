﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Linq;

namespace MVVM
{
    public class MainWindowModelView
    {
        private EditQuestions _editQuestions;
        private OpenQuiz _openQuiz;
        private MainWindow _mainwindow;
        private Settings _settings;


        public MainWindowModelView(MainWindow mainWindow)
        {
            _mainwindow = mainWindow;

            _mainwindow.NewQuiz += _mainwindow_NewQuiz;
            _mainwindow.OpenQuiz += _mainwindow_OpenQuiz;
            _mainwindow.EditQuiz += _mainwindow_EditQuiz;
            _mainwindow.Settings += _mainwindow_Settings;
            _mainwindow.DoubleClick += _mainwindow_DoubleClick;
            _mainwindow.FindButton += _mainwindow_FindButton;
        }

        private void _mainwindow_FindButton()
        {
            FindQuizes();
        }

        private void _mainwindow_DoubleClick()
        {
            DoubleClick();
        }

        private void _mainwindow_Settings()
        {
            Settings();
        }

        public static EnterName enterNameWindow { get; private set; }

        private void _mainwindow_EditQuiz()
        {
            EditQuiz();
        }

        private void _mainwindow_OpenQuiz()
        {
            OpenQuiz();
        }

        private void _mainwindow_NewQuiz()
        {
            NewQuiz();
        }

        public void FindQuizes()
        {
            

            string directoryPath = Properties.Settings.Default.Path;

            string SearchText = _mainwindow.Find;
            _mainwindow.ListBoxMain.Items.Clear();
            _mainwindow.FindTextBoxMain.Clear();

            try
            {
                
                string[] files = Directory.GetFiles(directoryPath, "*.db")
                                           .Where(file => Path.GetFileName(file).ToLower().Contains(SearchText.ToLower()))
                                           .ToArray();

                
                foreach (string file in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    string listItem = $"Name: {fileName}.db";
                    _mainwindow.ListBoxMain.Items.Add(listItem);
                }
            }
            catch (Exception ex)
            {
                
                System.Windows.Forms.MessageBox.Show($"Error: {ex.Message}");
            }
        }


        public void NewQuiz()
        {

            

            enterNameWindow = new EnterName();
            enterNameWindow.ShowDialog();
        }

        public void OpenQuiz()
        {

            OpenQuizViewModel viewModel = new OpenQuizViewModel();
            viewModel.Open();


        }



            








           



        private void AddDataToListBox(EditQuestions window, List<string> data)
        {
            foreach (string rowData in data)
            {
                
                string[] rowDataParts = rowData.Split('|');

                
                string formattedData = string.Format("Question: {0}| FirstAnswer: {1}| SecondAnswer: {2}| ThirdAnswer: {3}| FourthAnswer: {4}| CorrectlyAnswer: {5}",
                    rowDataParts[0].Trim(),
                    rowDataParts[1].Trim(),
                    rowDataParts[2].Trim(),
                    rowDataParts[3].Trim(),
                    rowDataParts[4].Trim(),
                    rowDataParts[5].Trim());

                
                window.ListBox_EditQuestions.Items.Add(formattedData);


                
            }
        }

        public void EditQuiz()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bazy danych (*.db)|*.db|Wszystkie pliki (*.*)|*.*";
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Title = "Wybierz plik bazy danych";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                EditQuestions window = new EditQuestions();
                string selectedFilePath = openFileDialog.FileName;
                string SelectedFileName = Path.GetFileNameWithoutExtension(selectedFilePath);
                window.Name_Quiz = SelectedFileName;


                

                string selectedFileName = Path.GetFileNameWithoutExtension(selectedFilePath) + ".db";

                string folderpath = Path.GetDirectoryName(selectedFilePath);
                
                

                List<string> data = Model.ReadData(folderpath, selectedFileName);

                AddDataToListBox(window, data);

                window.ShowDialog();


                
                
            }


        }

        public void Settings()
        {
            Settings window = new Settings();
            window.PathSettings = Properties.Settings.Default.Path;
            window.ShowDialog();
            
        }

        public void DoubleClick()
        {
            var selectedItem = _mainwindow.ListBoxMain.SelectedItem as string;
            string FileName = selectedItem;
            string Path = Properties.Settings.Default.Path;

            if (selectedItem != null)
            {
                
                FileName = FileName.Replace("Name: ", ""); 
                
                OpenQuizViewModel viewModel = new OpenQuizViewModel();
                viewModel.OpenWindowDoubleClick(Path, FileName);
            }
        }


        


    }
}
