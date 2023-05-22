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
using System.Windows.Threading;

namespace MVVM
{
    /// <summary>
    /// Logika interakcji dla klasy OpenQuiz.xaml
    /// </summary>
    public partial class OpenQuiz : Window
    {
        private OpenQuizViewModel _viewmodel;
        private DispatcherTimer timer;
        private TimeSpan timeRemaining;
        private bool timerStarted;
        public OpenQuiz()
        {
            
            InitializeComponent();
            _viewmodel = new OpenQuizViewModel();
            InitializeTimer();
            timerStarted = false;

        }
        public event Action SubmitAnswer;
        public event Action CheckAnswer;

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timeRemaining = TimeSpan.FromMinutes(5); // Ustaw czas trwania quizu
            TimerLabel.Content = "Czas: " + timeRemaining.ToString(@"mm\:ss");
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            timeRemaining -= TimeSpan.FromSeconds(1);
            TimerLabel.Content = "Czas: " + timeRemaining.ToString(@"mm\:ss");

            if (timeRemaining <= TimeSpan.Zero)
            {
                timer.Stop();
                MessageBox.Show("Gra zakończona!", "Koniec gry", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
         

        public string Question
        {
            get => QuestionLabelOpenQuiz.Content.ToString(); set => QuestionLabelOpenQuiz.Content = value ;
        }
        public string FirstAnswer
        {
            get => FirstAnswerRadioButtonOpenQuiz.Content.ToString(); set => FirstAnswerRadioButtonOpenQuiz.Content = value;
        }
        public string SecondAnswer
        {
            get => SecondAnswerRadioButtonOpenQuiz.Content.ToString(); set => SecondAnswerRadioButtonOpenQuiz.Content = value;
        }
        public string ThirdAnswer
        {
            get => ThirdAnswerRadioButtonOpenQuiz.Content.ToString(); set => ThirdAnswerRadioButtonOpenQuiz.Content = value;
        }
        public string FourthAnswer
        {
            get => FourthAnswerRadioButtonOpenQuiz.Content.ToString(); set => FourthAnswerRadioButtonOpenQuiz.Content = value;
        }
        public string CorrectlyAnswer { get; set; }

        public string Answer { get => GetSelectedAnswer(); }
        public Brush CorrectAnswerForeground { get => CorrectAnswerForeground; set => CorrectAnswerForeground = value; }
        public List<string> QuizData { get; internal set; }

        

        private string GetSelectedAnswer()
        {
            if (FirstAnswerRadioButtonOpenQuiz.IsChecked == true)
                return FirstAnswer;

            if (SecondAnswerRadioButtonOpenQuiz.IsChecked == true)
                return SecondAnswer;

            if (ThirdAnswerRadioButtonOpenQuiz.IsChecked == true)
                return ThirdAnswer;

            if (FourthAnswerRadioButtonOpenQuiz.IsChecked == true)
                return FourthAnswer;

            return string.Empty;
        }




        private void SubmitButtonOpenQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (!timerStarted)
            {
                timer.Start();
                timerStarted = true;
            }
            SubmitAnswer?.Invoke();
        }

        private void CheckButtonOpenQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (!timerStarted)
            {
                timer.Start();
                timerStarted = true;
            }
            CheckAnswer?.Invoke();
        }
    }
}
