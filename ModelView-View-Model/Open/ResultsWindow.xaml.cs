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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVVM
{
    /// <summary>
    /// Logika interakcji dla klasy ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        private OpenQuizViewModel _viewmodel;
        public ResultsWindow(OpenQuizViewModel viewModel)
        {
            InitializeComponent();
            _viewmodel = viewModel;
        }
        public string Wynik { get => WynikLabelResultsWindow.Content.ToString(); 
            set => WynikLabelResultsWindow.Content = value; }
        
        
    }
}
