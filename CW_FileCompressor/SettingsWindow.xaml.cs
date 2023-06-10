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
using System.Windows.Shapes;
using CW_FileCompressor.Properties.Langs;

namespace CW_FileCompressor
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;

            KeyDown += OnHotkeys;

            SetupOptions();
        }

        private void OnHotkeys(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
            }
        }

        private void SetupOptions()
        {
            switch (Properties.Settings.Default.languageCode)
            {
                case "en-US":
                    rdbLanguageEnglish.IsChecked = true;
                    break;
                case "uk-UA":
                    rdbLanguageUkrainian.IsChecked = true;
                    break;
            }

            switch (Properties.Settings.Default.colorTheme)
            {
                case "Light":
                    rdbThemeLight.IsChecked = true;
                    break;
                case "Dark":
                    rdbThemeDark.IsChecked = true;
                    break;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, Lang.SaveSettingsCautionLabel, Lang.SettingsLabel,
                MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void LanguageChecked(object sender, RoutedEventArgs e)
        {
            if (rdbLanguageEnglish.IsChecked == true) 
                Properties.Settings.Default.languageCode = "en-US";
            else if (rdbLanguageUkrainian.IsChecked == true)
                Properties.Settings.Default.languageCode = "uk-UA";
                
            Properties.Settings.Default.Save();
        }

        private void ThemeChecked(object sender, RoutedEventArgs e)
        {
            if (rdbThemeLight.IsChecked == true)
                Properties.Settings.Default.colorTheme = "Light";
            else if (rdbThemeDark.IsChecked == true)
                Properties.Settings.Default.colorTheme = "Dark";

            Properties.Settings.Default.Save();
        }
    }
}
