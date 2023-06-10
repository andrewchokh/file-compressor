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
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;
using CW_FileCompressor.Properties.Langs;
using MaterialDesignThemes.Wpf;
using CW_FileCompressor.Entities;

namespace CW_FileCompressor
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            KeyDown += OnHotkeys;
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
                    case Key.U:
                        tbxUsername.Focus();
                        break;
                    case Key.P:
                        psbPassword.Focus();
                        break;
                    case Key.R:
                        psbPasswordRepeat.Focus();
                        break;
                    case Key.E:
                        tbxEmail.Focus();
                        break;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tblTitle.Text = Lang.AuthorizationLabel;

            SetupFields();
        }

        private void btnPrimaryAction_Click(object sender, RoutedEventArgs e)
        {
            if (tblTitle.Text == Lang.AuthorizationLabel)
                ExecuteAuthorization();
            else if (tblTitle.Text == Lang.RegistartionLabel)
                ExecuteRegistration();
        }

        private void btnSecondaryAction_Click(object sender, RoutedEventArgs e)
        {
            if (tblTitle.Text == Lang.AuthorizationLabel)
                tblTitle.Text = Lang.RegistartionLabel;
            else if (tblTitle.Text == Lang.RegistartionLabel)
                tblTitle.Text = Lang.AuthorizationLabel;

            SetupFields();
        }

        private void SetupFields()
        {
            stpFields.Children.Clear();

            var resourceDictionary = Application.Current.Resources;

            if (tblTitle.Text == Lang.AuthorizationLabel)
            {
                tbxEmail = new TextBox
                {
                    Name = "tbxEmail",
                    ToolTip = Lang.EmailToolTip,
                    Style = (Style)resourceDictionary["CustomMaterialDesignFloatingHintTextBox"]
                };
                HintAssist.SetHint(tbxEmail, Lang.EmailHint);

                psbPassword = new PasswordBox
                {
                    Name = "tbxPassword",
                    ToolTip = Lang.PasswordToolTip,
                    Style = (Style)resourceDictionary["CustomMaterialDesignFloatingHintPasswordBox"]
                };
                HintAssist.SetHint(psbPassword, Lang.PasswordHint);

                stpFields.Children.Add(tbxEmail);
                stpFields.Children.Add(psbPassword);

                btnPrimaryAction.Content = Lang.ButtonLogInContent;
                btnSecondaryAction.Content = Lang.ButtonSignUpContent;
            }
            else if (tblTitle.Text == Lang.RegistartionLabel)
            {
                tbxUsername = new TextBox
                {
                    Name = "tbxUsername",
                    ToolTip = Lang.UsernameToolTip,
                    Style = (Style)resourceDictionary["CustomMaterialDesignFloatingHintTextBox"]
                };
                HintAssist.SetHint(tbxUsername, Lang.UsernameHint);

                tbxEmail = new TextBox
                {
                    Name = "tbxEmail",
                    ToolTip = Lang.EmailToolTip,
                    Style = (Style)resourceDictionary["CustomMaterialDesignFloatingHintTextBox"]
                };
                HintAssist.SetHint(tbxEmail, Lang.EmailHint);

                psbPassword = new PasswordBox
                {
                    Name = "tbxPassword",
                    ToolTip = Lang.PasswordToolTip,
                    Style = (Style)resourceDictionary["CustomMaterialDesignFloatingHintPasswordBox"]
                };
                HintAssist.SetHint(psbPassword, Lang.PasswordHint);

                psbPasswordRepeat = new PasswordBox
                {
                    Name = "tbxPasswordRepeat",
                    ToolTip = Lang.PasswordToolTip,
                    Style = (Style)resourceDictionary["CustomMaterialDesignFloatingHintPasswordBox"]
                };
                HintAssist.SetHint(psbPasswordRepeat, Lang.RepeatPasswordHint);

                stpFields.Children.Add(tbxUsername);
                stpFields.Children.Add(tbxEmail);
                stpFields.Children.Add(psbPassword);
                stpFields.Children.Add(psbPasswordRepeat);

                btnPrimaryAction.Content = Lang.ButtonSignUpContent;
                btnSecondaryAction.Content = Lang.ButtonLogInContent;
            }
        }

        private void ExecuteAuthorization()
        {
            if (psbPassword.Password.Length < 8 ||
                tbxEmail.Text.Length < 5 ||
                !tbxEmail.Text.Contains('@') ||
                !tbxEmail.Text.Contains('.'))
            {
                MessageBox.Show(this, Lang.WrongInputLabel, Lang.ErrorOccuredLabel,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string filePath = "./Data/users.json";

            if (!File.Exists(filePath))
            {
                MessageBox.Show(this, string.Format(Lang.CannotReadFileLabel, filePath), Lang.ErrorOccuredLabel,
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string jsonData;
            List<User> userList;

            jsonData = File.ReadAllText(filePath);

            userList = JsonConvert.DeserializeObject<List<User>>(jsonData) ?? new List<User>();

            foreach (var item in userList)
            {
                if (item.Email == tbxEmail.Text &&
                    item.Password == HashString(psbPassword.Password, tbxEmail.Text))
                {
                    var mainWindow = new MainWindow();
                    mainWindow.tblUsername.Text = item.Username;
                    mainWindow.Show();
                    this.Close();
                    return;
                }
            }

            MessageBox.Show(this, Lang.CannotFindAccountLabel, Lang.ErrorOccuredLabel,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ExecuteRegistration()
        {
            if (tbxUsername.Text.Length == 0 ||
                psbPassword.Password.Length < 8 ||
                psbPassword.Password != psbPasswordRepeat.Password ||
                tbxEmail.Text.Length < 5 ||
                !tbxEmail.Text.Contains('@') ||
                !tbxEmail.Text.Contains('.'))
            {
                MessageBox.Show(this, Lang.WrongInputLabel, Lang.ErrorOccuredLabel,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = new User
            {
                Username = tbxUsername.Text,
                Password = HashString(psbPassword.Password, tbxEmail.Text),
                Email = tbxEmail.Text,
            };

            string filePath = "./Data/users.json";

            if (!File.Exists(filePath))
            {
                MessageBox.Show(this, string.Format(Lang.CannotReadFileLabel, filePath), Lang.ErrorOccuredLabel,
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // This variables must be global for the function for write new data in file.
            string jsonData;
            List<User> userList;

            jsonData = File.ReadAllText(filePath);

            userList = JsonConvert.DeserializeObject<List<User>>(jsonData) ?? new List<User>();

            foreach (var item in userList)
            {
                if (item.Email == tbxEmail.Text)
                {
                    MessageBox.Show(this, Lang.AccountExistsLabel, Lang.ErrorOccuredLabel,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            userList.Add(user);

            jsonData = JsonConvert.SerializeObject(userList);
            File.WriteAllText(filePath, jsonData);

            tblTitle.Text = Lang.AuthorizationLabel;

            SetupFields();
        }

        private string HashString(string text, string salt = "")
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            using (var sha = SHA256.Create())
            {
                byte[] textBytes = Encoding.UTF8.GetBytes(text + salt);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", string.Empty);

                return hash;
            }
        }
    }
}
