using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CW_FileCompressor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var langCode = CW_FileCompressor.Properties.Settings.Default.languageCode;
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(langCode);

            var theme = new ResourceDictionary();
            switch (CW_FileCompressor.Properties.Settings.Default.colorTheme)
            {
                case "Light":
                    theme.Source = new Uri("pack://application:,,,/Dictionaries/LightColorDictionary.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "Dark":
                    theme.Source = new Uri("pack://application:,,,/Dictionaries/DarkColorDictionary.xaml", UriKind.RelativeOrAbsolute);
                    break;
            }
            Resources.MergedDictionaries.Add(theme);

            base.OnStartup(e);
        }
    }
}
