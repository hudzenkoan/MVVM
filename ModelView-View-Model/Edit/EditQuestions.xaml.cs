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
        public string Question { get => QuestionTextBoxEditQuestions.Text; set => QuestionTextBoxEditQuestions.Text = value; }
        public string FirstAnswer { get => FirstAnswerTextBoxEditQuestions.Text; set => FirstAnswerTextBoxEditQuestions.Text = value; }
        public string SecondAnswer { get => SecondAnswerTextBoxEditQuestions.Text; set => SecondAnswerTextBoxEditQuestions.Text = value; }
        public string ThirdAnswer { get => ThirdAnswerTextBoxEditQuestions.Text; set => ThirdAnswerTextBoxEditQuestions.Text = value; }
        public string FourthAnswer { get => FourthAnswerTextBoxEditQuestions.Text; set => FourthAnswerTextBoxEditQuestions.Text = value; }
        public string CorrectlyAnswer { get => GetSelectedRadioButtonText(); set
            {
                if (value == FirstAnswerTextBoxEditQuestions.Text)
                {
                    FirstRadioButtonEdtiQuestions.IsChecked = true;
                }
                else if (value == SecondAnswerTextBoxEditQuestions.Text)
                {
                    SecondRadioButtonEdtiQuestions.IsChecked = true;
                }
                else if (value == ThirdAnswerTextBoxEditQuestions.Text)
                {
                    ThirdRadioButtonEdtiQuestions.IsChecked = true;
                }
                else if (value == FourthAnswerTextBoxEditQuestions.Text)
                {
                    FourthRadioButtonEdtiQuestions.IsChecked = true;
                }
            }
        }
        public int ListBoxIndex { get => ListBox_EditQuestions.SelectedIndex; }


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

        public void ClearForm()
        {
            QuestionTextBoxEditQuestions.Clear();
            FirstAnswerTextBoxEditQuestions.Clear();
            SecondAnswerTextBoxEditQuestions.Clear();
            ThirdAnswerTextBoxEditQuestions.Clear();
            FourthAnswerTextBoxEditQuestions.Clear();
            FirstRadioButtonEdtiQuestions.IsChecked = false;
            SecondRadioButtonEdtiQuestions.IsChecked = false;
            ThirdRadioButtonEdtiQuestions.IsChecked = false;
            FourthRadioButtonEdtiQuestions.IsChecked = false;
        }

        private void ListBox_EditQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBox_EditQuestions.SelectedItem != null)
            {
                string selectedQuestion = ListBox_EditQuestions.SelectedItem.ToString();

                string[] questionParts = selectedQuestion.Split(new string[] { "| " }, StringSplitOptions.None);

                if (questionParts.Length == 6)
                {
                    Question = questionParts[0].Replace("Question: ", "");
                    FirstAnswer = questionParts[1].Replace("FirstAnswer: ", "");
                    SecondAnswer = questionParts[2].Replace("SecondAnswer: ", "");
                    ThirdAnswer = questionParts[3].Replace("ThirdAnswer: ", "");
                    FourthAnswer = questionParts[4].Replace("FourthAnswer: ", "");
                    CorrectlyAnswer = questionParts[5].Replace("CorrectlyAnswer: ", "");
                }
            }
        }
    }
}
