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
using AdminPanelComiverse.Classes;
using ComicsApi.Classes;
using ComicsApi.Models;
using RestSharp;

namespace AdminPanelComiverse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RestClient apiClient = ApiBuilder.GetInstance();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnEnterClick(object sender, RoutedEventArgs e)
        {
            UserAuthData userAuthData = new UserAuthData()
            {
                Login = tbLogin.Text,
                Password = CreateMD5(pbPassword.Password)
            };
            var response = apiClient.Post<User>(new RestRequest("User/Auth")
                .AddBody(userAuthData));
            if (response == null)
            {
                MessageBox.Show("Пользователь не найден");
            }
            else
            {
                if (response.Role == true)
                {
                    MessageBox.Show("Отказано в доступе");
                }
                else
                {
                    AdminWindow adminWindow = new AdminWindow();
                    Hide();
                    adminWindow.Show();
                }
            }
        }
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}