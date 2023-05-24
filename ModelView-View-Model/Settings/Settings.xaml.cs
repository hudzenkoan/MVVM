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
    /// Logika interakcji dla klasy Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private SettingsViewModel _settingsViewModel;
        public string PathSettings {get => PathTextBoxSettings.Text; set => PathTextBoxSettings.Text = value;}
        public Settings()
        {
            InitializeComponent();
            _settingsViewModel = new SettingsViewModel(this);
            PathSettings = Properties.Settings.Default.Path;
        }
        public event Action EditPath;
        public event Action SaveSettings;
        public event Action ExitSettings;

        private void EditButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            EditPath?.Invoke();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings?.Invoke();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ExitSettings?.Invoke();
        }
    }
}
