using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MVVM
{
    public class SettingsViewModel
    {
        private Settings _settings;
        private string Path { get; set; }
        public SettingsViewModel(Settings settings){
            _settings = settings;

            _settings.EditPath += _settings_EditPath;
            _settings.SaveSettings += _settings_SaveSettings;
            _settings.ExitSettings += _settings_ExitSettings;
        }

        private void _settings_ExitSettings()
        {
            ExitSettings();
        }

        private void _settings_SaveSettings()
        {
            SaveSettings();
        }

        private void _settings_EditPath()
        {
            EditPath();
        }

        public void ExitSettings()
        {
            _settings.Close();
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.Path = Path;
            Properties.Settings.Default.Save();
            _settings.Close();
        }
        public void EditPath()
        {
            FolderBrowserDialog folderdialog = new FolderBrowserDialog();
            DialogResult result = folderdialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string selectedFolder = folderdialog.SelectedPath;
                Path = selectedFolder;
                _settings.PathSettings = selectedFolder;
                
            }
        }
    }

}
