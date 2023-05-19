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
    /// Logika interakcji dla klasy EnterName.xaml
    /// </summary>
    public partial class EnterName : Window
    {
        private NewQuizViewModel _viewmodel;
        public EnterName()
        {
            InitializeComponent();
            _viewmodel = new NewQuizViewModel(this);
        }

        public event Action CancelCreateQuiz;
        public event Action ContinueCreateQuiz;

        public string Name { get => EnterNameTextBox.Text;  }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContinueCreateQuiz?.Invoke();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CancelCreateQuiz?.Invoke();
        }
    }
}
