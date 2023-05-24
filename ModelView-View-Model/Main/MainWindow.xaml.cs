using System;
using System.Collections.Generic;
using System.IO;
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
        public string FolderPath;
        private string SearchPattern = "*.db";
        public string[] files;
        public MainWindow()
        {
            InitializeComponent();
            _modelView = new MainWindowModelView(this);
            FolderPath = Properties.Settings.Default.Path;
            if (!string.IsNullOrEmpty(FolderPath) && Directory.Exists(FolderPath))
            {
                files = Directory.GetFiles(FolderPath, SearchPattern);
                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        string fileName = System.IO.Path.GetFileName(file);
                        ListBoxMain.Items.Add($"Name: {fileName}");
                    }
                }
            }
            else
            {

            }
            
        }
        public event Action NewQuiz;
        public event Action OpenQuiz;
        public event Action EditQuiz;
        public event Action Settings;
        public event Action DoubleClick;

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

        private void SettingsButtonMain_Click(object sender, RoutedEventArgs e)
        {
            Settings?.Invoke();
        }

        private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DoubleClick?.Invoke();
        }
    }
}
