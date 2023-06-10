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
using CW_FileCompressor.Algorithms;
using CW_FileCompressor.Properties.Langs;
using Microsoft.Win32;

namespace CW_FileCompressor
{
    /// <summary>
    /// Interaction logic for CompressionWindow.xaml
    /// </summary>
    public partial class CompressionWindow : Window
    {
        // We can't scan the algorithm folder. So, the list is necessary.
        string[] cmbCompressionItems = { "LZ77", "RLE", "BWT" };

        public CompressionWindow()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;

            KeyDown += OnHotkeys;

            SetupAlgorithms();
        }

        private void OnHotkeys(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
            }

            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.F:
                        tbxFilePath.Focus();
                        break;
                    case Key.S:
                        tbxSaveFilePath.Focus();
                        break;
                    case Key.C:
                        cmbCompression.Focus();
                        break;
                }
            }
        }

        private void SetupAlgorithms()
        {
            cmbCompression.Items.Clear();

            for (int i = 0; i < cmbCompressionItems.Length; i++)
            {
                cmbCompression.Items.Add(new Entities.ComboBoxItem
                {
                    Text = cmbCompressionItems[i],
                    Value = i,
                });
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (this.Title == Lang.CompressFileLabel &&
                (string.IsNullOrEmpty(tbxFilePath.Text) ||
                string.IsNullOrEmpty(tbxSaveFilePath.Text) ||
                string.IsNullOrEmpty(cmbCompression.Text)) ||
                this.Title == Lang.DecompressFileLabel &&
                (string.IsNullOrEmpty(tbxFilePath.Text) ||
                string.IsNullOrEmpty(tbxSaveFilePath.Text)))
            {
                MessageBox.Show(this, Lang.WrongInputLabel, Lang.ErrorOccuredLabel,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!File.Exists(tbxFilePath.Text))
            {
                MessageBox.Show(this, string.Format(Lang.CannotReadFileLabel, tbxFilePath.Text), Lang.ErrorOccuredLabel,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            byte[] fileBytes = File.ReadAllBytes(tbxFilePath.Text);
            byte[]? outputFileBytes = null;

            try
            {
                if (this.Title == Lang.CompressFileLabel)
                    outputFileBytes = CompressBytes(fileBytes);
                else if (this.Title == Lang.DecompressFileLabel)
                    outputFileBytes = DecompressBytes(fileBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Lang.ErrorOccuredLabel,
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.Close();

            File.WriteAllBytes(tbxSaveFilePath.Text, outputFileBytes);

            if (this.Title == Lang.CompressFileLabel)
                MessageBox.Show(this, Lang.SuccessCompressLabel, Lang.DoneLabel,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            else if (this.Title == Lang.DecompressFileLabel)
                MessageBox.Show(this, Lang.SuccessDecompressLabel, Lang.DoneLabel,
                    MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private byte[] CompressBytes(byte[] fileBytes)
        {
            byte[] compressedFileBytes;

            switch (cmbCompression.SelectedItem.ToString())
            {
                case "LZ77":
                    compressedFileBytes = LZ77.Compress(fileBytes);
                    break;
                case "RLE":
                    compressedFileBytes = RLE.Compress(fileBytes);
                    break;
                case "BWT":
                    compressedFileBytes = BWT.Compress(fileBytes);
                    break;
                default:
                    throw new ArgumentException(Lang.NoAlgorithmLabel);
            }

            return compressedFileBytes;
        }

        private byte[] DecompressBytes(byte[] fileBytes)
        {
            byte[] decompressedFileBytes;

            switch (tbxFilePath.Text.Split('.').Last())
            {
                case "clz77":
                    decompressedFileBytes = LZ77.Decompress(fileBytes);
                    break;
                case "crle":
                    decompressedFileBytes = RLE.Decompress(fileBytes);
                    break;
                case "cbwt":
                    decompressedFileBytes = BWT.Decompress(fileBytes);
                    break;
                default:
                    throw new ArgumentException(Lang.NoAlgorithmLabel);
            }

            return decompressedFileBytes;
        }

        private void btnSelectFilePath_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.ShowDialog();

            if (string.IsNullOrEmpty(ofd.FileName))
                return;

            tbxFilePath.Text = ofd.FileName;
        }

        private void btnSelectSaveFilePath_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.ShowDialog();

            if (string.IsNullOrEmpty(ofd.FileName))
                return;

            tbxSaveFilePath.Text = ofd.FileName;
        }

        private void cmbCompression_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Title != Lang.CompressFileLabel || string.IsNullOrEmpty(tbxSaveFilePath.Text))
                return;

            string selectedItemName = tbxSaveFilePath.Text;

            for (int i = selectedItemName.Length - 1; i > 0; i--)
            {
                if (selectedItemName[i] != '.')
                    selectedItemName = selectedItemName.Remove(i);
                else
                {
                    selectedItemName = selectedItemName.Remove(i);
                    break;
                }
                    
            }

            switch (cmbCompression.SelectedItem.ToString())
            {
                case "LZ77":
                    selectedItemName += ".clz77";
                    break;
                case "RLE":
                    selectedItemName += ".crle";
                    break;
                case "BWT":
                    selectedItemName += ".cbwt";
                    break;
                default:
                    selectedItemName += ".c";
                    break;
            }

            tbxSaveFilePath.Text = selectedItemName;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (this.Title == Lang.DecompressFileLabel)
                cmbCompression.Visibility = Visibility.Collapsed;
        }
    }
}
