using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace MVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    //View
    public partial class MainWindow : Window
    {
        private MainWindowModelView _modelView;
        public MainWindow()
        {
            InitializeComponent();
            _modelView = new MainWindowModelView(this);
        }
        public event Action NewQuiz;
        public event Action OpenQuiz;
        public event Action EditQuiz;

        public string Find { 
            get => FindTextBoxMain.Text; 
        }

        private void NewButtonMain_Click(object sender, RoutedEventArgs e)
        {
            NewQuiz?.Invoke();
        }

        private void EditButtonMain_Click(object sender, RoutedEventArgs e)
        {
            EditQuiz?.Invoke();
        }

        private void OpenButtonMain_Click(object sender, RoutedEventArgs e)
        {
            OpenQuiz?.Invoke();
        }
    }
}
