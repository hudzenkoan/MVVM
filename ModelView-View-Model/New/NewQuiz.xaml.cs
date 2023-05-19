using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace MVVM
{
    /// <summary>
    /// Logika interakcji dla klasy NewQuiz.xaml
    /// </summary>
    public partial class NewQuiz : Window
    {
        private NewQuizViewModel _viewmodel;
        public NewQuiz()
        {
            InitializeComponent();
            _viewmodel = new NewQuizViewModel(this);

        }

        public event Action AddQuestions;
        public event Action DeleteQuestions;
        public event Action SaveQuiz;

        public string Name { 
            get => NameTextBoxNewQuiz.Text;
            set => NameTextBoxNewQuiz.Text = value;
        }
        public string Question { get => QuestionTextBoxNewQuiz.Text; }
        public string FirstAnswer { get => FirstAnswerTextBoxNewQuiz.Text; }
        public string SecondAnswer { get => SecondAnswerTextBoxNewQuiz.Text; }
        public string ThirdAnswer { get => ThirdAnswerTextBoxNewQuiz.Text; }
        public string FourthAnswer { get => FourthAnswerTextBoxNewQuiz.Text; }
        public string CorrectlyAnswer { get => GetSelectedRadioButtonText(); }

        private void AddButtonNewQuiz_Click(object sender, RoutedEventArgs e)
        {
            AddQuestions?.Invoke();
        }

        private void DeleteButtonNewQuiz_Click(object sender, RoutedEventArgs e)
        {
            DeleteQuestions?.Invoke();
        }

        private void SaveButtonNewQuiz_Click(object sender, RoutedEventArgs e)
        {
            SaveQuiz?.Invoke();
        }

        private string GetSelectedRadioButtonText()
        {
            if (FirstAnswerRadioButton.IsChecked == true)
            {

                return FirstAnswerTextBoxNewQuiz.Text;
            }
            else if (SecondAnswerRadioButton.IsChecked == true)
            {
                return SecondAnswerTextBoxNewQuiz.Text;
            }
            else if (ThirdAnswerRadioButton.IsChecked == true)
            {

                return ThirdAnswerTextBoxNewQuiz.Text;
            }
            else if (FourthAnswerRadioButton.IsChecked == true)
            {

                return FourthAnswerTextBoxNewQuiz.Text;
            }
            else
            {
                return "Error!";
            }
        }
    }
}
