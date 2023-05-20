using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace MVVM
{
    public class MainWindowModelView
    {
        private EditQuestions _editQuestions;
        private MainWindow _mainwindow;
        
        public MainWindowModelView(MainWindow mainWindow)
        {
            _mainwindow = mainWindow;

            _mainwindow.NewQuiz += _mainwindow_NewQuiz;
            _mainwindow.OpenQuiz += _mainwindow_OpenQuiz;
            _mainwindow.EditQuiz += _mainwindow_EditQuiz;
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

        public void NewQuiz()
        {

            //var window = new NewQuiz();

            enterNameWindow = new EnterName();
            enterNameWindow.ShowDialog();
        }

        public void OpenQuiz()
        {
            var window = new OpenQuiz();
            window.ShowDialog();
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
                
                window.ShowDialog();

                // Считывание данных из базы данных
            }


        }




        //public string Question { get; set; }
        //public string FirstAnswer{ get; set; }
        //public string SecondAnswer { get; set; }
        //public string ThirdAnswer{ get; set; }
        //public string FourthAnswer { get; set; }
        //public string Name_Quiz { get; set; }


    }
}
