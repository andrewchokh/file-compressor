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
using System.IO;
using CW_FileCompressor.Properties.Langs;

namespace CW_FileCompressor
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;

            KeyDown += OnHotkeys;
        }

        private void OnHotkeys(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Close();
                    break;
            }
        }

        private void txtDoc_Loaded(object sender, RoutedEventArgs e)
        {
            string filePath = "./Data/about.txt";

            try
            {
                tblDoc.Text = File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format(Lang.CannotReadFileLabel, ex.Message), Lang.ErrorOccuredLabel,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
