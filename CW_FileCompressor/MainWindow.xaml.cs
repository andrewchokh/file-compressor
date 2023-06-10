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
using System.IO;
using System.Diagnostics;
using System.Resources;
using Ookii.Dialogs.Wpf;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using CW_FileCompressor.Properties.Langs;
using CW_FileCompressor.Entities;

namespace CW_FileCompressor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Starting directory must end with slash. Otherwise, the path will be wrong.
        string explorerPath = $"{AppDomain.CurrentDomain.BaseDirectory[0]}:\\";

        public MainWindow()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Application.Current.MainWindow = this;

            LoadSidePanelItems();
            LoadActionButtonItems();

            KeyDown += OnHotkeys;

            // This needs here for correct data display. Don't remove.
            tblFileSize.Text = string.Format(Lang.FileSizeLabel, "-");
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
                    case Key.C:
                        btnCompress_Click(sender, e);
                        break;
                    case Key.D:
                        btnDecompress_Click(sender, e);
                        break;
                    case Key.A:
                        btnAbout_Click(sender, e);
                        break;
                    case Key.S:
                        btnSettings_Click(sender, e);
                        break;
                }
            }
        }

        private void LoadSidePanelItems() 
        {
            var resourceDictionary = Application.Current.Resources;

            string filePath = "./Data/side_menu_items.json";
            string jsonData;

            try
            {
                jsonData = File.ReadAllText(filePath);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(this, string.Format(Lang.NoFileFoundLabel, filePath), Lang.ErrorOccuredLabel,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            var itemList = JsonConvert.DeserializeObject<List<SideMenuItem>>(jsonData)
                    ?? new List<SideMenuItem>();

            for (int i = 0; i < itemList.Count; i++)
            {
                switch (itemList[i].Name)
                {
                    case "Desktop":
                        itemList[i].Content = Lang.DesktopLabel;
                        break;
                    case "Documents":
                        itemList[i].Content = Lang.DocumentsLabel;
                        break;
                    case "Downloads":
                        itemList[i].Content = Lang.DownloadsLabel;
                        break;
                    case "Pictures":
                        itemList[i].Content = Lang.PicturesLabel;
                        break;
                    case "Videos":
                        itemList[i].Content = Lang.VideosLabel;
                        break;
                    case "Music":
                        itemList[i].Content = Lang.MusicLabel;
                        break;
                    case "DiskC":
                        itemList[i].Content = Lang.DiskCLabel;
                        break;
                    case "DiskD":
                        itemList[i].Content = Lang.DiskDLabel;
                        break;
                    case "DiskE":
                        itemList[i].Content = Lang.DiskELabel;
                        break;
                }

                var item = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Children =
                    {
                        new PackIcon 
                        {   Kind = (PackIconKind)itemList[i].IconID, 
                            Foreground=(Brush)resourceDictionary["PrimaryLightBrush"], 
                            Width=16, Height=16, 
                            HorizontalAlignment=HorizontalAlignment.Center,
                            VerticalAlignment=VerticalAlignment.Center,
                        
                        },
                        new Button 
                        {
                            Name = "btn" + itemList[i].Name, 
                            Content = itemList[i].Content, 
                            Foreground=(Brush)resourceDictionary["PrimaryLightBrush"],
                            BorderBrush=null,
                            Background=null,
                            FontSize=10
                        }
                    }
                };

                (item.Children[1] as Button).Click += SideMenuButton_Click;

                if (itemList[i].CategoryIndex == 0)
                    stpLibraries.Children.Add(item);
                else if (itemList[i].CategoryIndex == 1)
                    stpDisks.Children.Add(item);
                else
                    stpSidePanel.Children.Add(item);
            }
        }

        private void LoadActionButtonItems()
        {
            var resourceDictionary = Application.Current.Resources;

            string filePath = "./Data/action_button_items.json";
            string jsonData;

            try
            {
                jsonData = File.ReadAllText(filePath);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(this, string.Format(Lang.NoFileFoundLabel, filePath), Lang.ErrorOccuredLabel,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            var itemList = JsonConvert.DeserializeObject<List<ActionButtonItem>>(jsonData)
                ?? new List<ActionButtonItem>();

            for (int i = 0; i < itemList.Count; i++)
            {
                switch (itemList[i].ButtonID)
                {
                    case 0:
                        itemList[i].Content = Lang.CompressLabel;
                        break;
                    case 1:
                        itemList[i].Content = Lang.DecompressLabel;
                        break;
                    case 2:
                        itemList[i].Content = Lang.AboutLabel;
                        break;
                    case 3:
                        itemList[i].Content = Lang.SettingsLabel;
                        break;
                }

                var item = new StackPanel
                {
                    Children =
                    {
                        new Button
                        {
                            Name = "btn" + itemList[i].Name,
                            BorderBrush = null,
                            Background = null,
                            Height = 48,
                            Content = new PackIcon
                            {   
                                Kind = (PackIconKind)itemList[i].IconID,
                                Foreground=(Brush)resourceDictionary["AccentMidBrush"],
                                Margin = new Thickness(10, 0, 10, 0),
                                Width = 48, Height = 48,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,

                            },
                        },
                        new TextBlock
                        {
                            Text = itemList[i].Content,
                            Foreground = (Brush)Application.Current.Resources["AccentMidBrush"],
                            HorizontalAlignment = HorizontalAlignment.Center
                        }
                    }
                };

                (item.Children[0] as Button).Click += ActionButton_Click;

                wrpActionButtons.Children.Add(item);
            }
        }

        private void UpdateExplorer()
        {
            lsvExplorerPath.Items.Clear();

            if (explorerPath == string.Empty) return;

            if (!Directory.Exists(explorerPath))
            {
                MessageBox.Show(this, string.Format(Lang.NoDirLabel, explorerPath), Lang.ErrorOccuredLabel,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                explorerPath = string.Empty;
                tbxPath.Text = string.Empty;
                return;
            }

            var fileList = new DirectoryInfo(explorerPath);

            var files = fileList.GetFiles()
                .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));

            var dirs = fileList.GetDirectories()
                .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));

            foreach (FileInfo file in files)
                lsvExplorerPath.Items.Add(file.Name);

            foreach (DirectoryInfo dir in dirs)
                lsvExplorerPath.Items.Add(dir.Name);

            if (lsvExplorerPath.Items.Count <= 999)
                tblItemsCount.Text = string.Format(Lang.ElementsLabel, lsvExplorerPath.Items.Count);
            else
                tblItemsCount.Text = string.Format(Lang.ElementsLabel, "999+");

            if (explorerPath.Last() == '\\')
                tbxPath.Text = explorerPath.Substring(0, explorerPath.Length - 1);
            else
                tbxPath.Text = explorerPath;
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            UpdateExplorer();
        }

        private void lsvExplorerPath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string path = $"{explorerPath}\\{lsvExplorerPath.SelectedItems[0]}";

            if (File.Exists(path))
            {
                try
                {
                    // Using shell execute is necessary. Otherwise, get MicrosoftWin32 exception.
                    Process process = new Process();
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = path;
                    process.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, Lang.ErrorOccuredLabel,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else if (Directory.Exists(path))
                explorerPath = path;

            UpdateExplorer();
        }

        private void lsvExplorerPath_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string path = $"{explorerPath}\\{lsvExplorerPath.SelectedItems[0]}";

                long fileSize = new FileInfo(path).Length;

                tblFileSize.Text = string.Format(Lang.FileSizeLabel, fileSize);
            }
            catch
            {
                tblFileSize.Text = string.Format(Lang.FileSizeLabel, "-");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (tbxPath.Text.Last() == ':') return;

            explorerPath = tbxPath.Text
                .Substring(0, tbxPath.Text.LastIndexOf("\\"));

            UpdateExplorer();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var fbd = new VistaFolderBrowserDialog();
            fbd.ShowDialog();

            if (string.IsNullOrEmpty(fbd.SelectedPath))
                return;

            explorerPath = fbd.SelectedPath;

            UpdateExplorer();
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = (sender as Button).Name;

            switch (buttonName)
            {
                case "btnCompress":
                    btnCompress_Click(sender, e);
                    break;
                case "btnDecompress":
                    btnDecompress_Click(sender, e);
                    break;
                case "btnAbout":
                    btnAbout_Click(sender, e);
                    break;
                case "btnSettings":
                    btnSettings_Click(sender, e);
                    break;
            }
        }

        private void SideMenuButton_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = (sender as Button).Name;
            string userFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            switch (buttonName)
            {
                case "btnDesktop":
                    explorerPath = $"{userFolderPath}\\Desktop";
                    break;
                case "btnDocuments":
                    explorerPath = $"{userFolderPath}\\Documents";
                    break;
                case "btnDownloads":
                    explorerPath = $"{userFolderPath}\\Downloads";
                    break;
                case "btnPictures":
                    explorerPath = $"{userFolderPath}\\Pictures";
                    break;
                case "btnVideos":
                    explorerPath = $"{userFolderPath}\\Videos";
                    break;
                case "btnMusic":
                    explorerPath = $"{userFolderPath}\\Music";
                    break;
                case "btnDiskC":
                    explorerPath = "C:\\";
                    break;
                case "btnDiskD":
                    explorerPath = "D:";
                    break;
                case "btnDiskE":
                    explorerPath = "E:";
                    break;
            }

            UpdateExplorer();
        }

        private void btnCompress_Click(object sender, RoutedEventArgs e)
        {
            var compressionWindow = new CompressionWindow();

            compressionWindow.Title = Lang.CompressFileLabel;
            compressionWindow.tblTitle.Text = Lang.CompressFileLabel;

            if (lsvExplorerPath.SelectedItems.Count > 0)
            {
                string path = $"{explorerPath}\\{lsvExplorerPath.SelectedItems[0]}";

                if (File.Exists(path))
                {
                    compressionWindow.tbxFilePath.Text = path;
                    compressionWindow.tbxSaveFilePath.Text = path;
                }
            }

            compressionWindow.ShowDialog();
        }

        private void btnDecompress_Click(object sender, RoutedEventArgs e)
        {
            var compressionWindow = new CompressionWindow();

            compressionWindow.Title = Lang.DecompressFileLabel;
            compressionWindow.tblTitle.Text = Lang.DecompressFileLabel;

            if (lsvExplorerPath.SelectedItems.Count > 0)
            {
                string path = $"{explorerPath}\\{lsvExplorerPath.SelectedItems[0]}";

                if (File.Exists(path))
                {
                    compressionWindow.tbxFilePath.Text = path;
                    compressionWindow.tbxSaveFilePath.Text = $"{path}.FORMAT";
                }
            }

            compressionWindow.ShowDialog();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }
    }
}
