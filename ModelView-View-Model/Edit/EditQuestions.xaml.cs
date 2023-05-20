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
    /// Logika interakcji dla klasy EditQuestions.xaml
    /// </summary>
    public partial class EditQuestions : Window
    {
        private EditQuestionsViewModel _model;
        public EditQuestions()
        {
            InitializeComponent();
            _model = new EditQuestionsViewModel(this);
        }



        public string Name_Quiz { get => NameTextBoxEditQuestions.Text; set => NameTextBoxEditQuestions.Text = value; }
        public string Question { get => QuestionTextBoxEditQuestions.Text; }
        public string FirstAnswer { get => FirstAnswerTextBoxEditQuestions.Text; }
        public string SecondAnswer { get => SecondAnswerTextBoxEditQuestions.Text; }
        public string ThirdAnswer { get => ThirdAnswerTextBoxEditQuestions.Text; }
        public string FourthAnswer { get => FourthAnswerTextBoxEditQuestions.Text; }
        public string CorrectlyAnswer { get => GetSelectedRadioButtonText(); }


        public event Action SaveQuiz;
        public event Action AddQuestion;
        public event Action DeleteQuestions;

        private void AddButtonEditQuestions_Click(object sender, RoutedEventArgs e)
        {
            AddQuestion?.Invoke();
        }

        private void DeleteButtonEditQuestions_Click(object sender, RoutedEventArgs e)
        {
            DeleteQuestions?.Invoke();
        }

        private void SaveButtonEditQuestions_Click(object sender, RoutedEventArgs e)
        {
            SaveQuiz?.Invoke();
        }
        private string GetSelectedRadioButtonText()
        {
            if (FirstRadioButtonEdtiQuestions.IsChecked == true)
            {

                return FirstAnswerTextBoxEditQuestions.Text;
            }
            else if (SecondRadioButtonEdtiQuestions.IsChecked == true)
            {
                return SecondAnswerTextBoxEditQuestions.Text;
            }
            else if (ThirdRadioButtonEdtiQuestions.IsChecked == true)
            {

                return ThirdAnswerTextBoxEditQuestions.Text;
            }
            else if (FourthRadioButtonEdtiQuestions.IsChecked == true)
            {

                return FourthAnswerTextBoxEditQuestions.Text;
            }
            else
            {
                return "Error!";
            }
        }
    }
}
