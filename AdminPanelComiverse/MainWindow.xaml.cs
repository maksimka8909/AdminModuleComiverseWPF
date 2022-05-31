using System.ComponentModel;
using System.Text;
using System.Windows;
using AdminPanelComiverse.Classes;
using ComicsApi.Classes;
using ComicsApi.Models;
using RestSharp;

namespace AdminPanelComiverse
{
    public partial class MainWindow : Window
    {
        private RestClient apiClient = ApiBuilder.GetInstance();
        
        /// <summary>
        /// инициализация окна MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Обработчик нажатия кнопки выхода из приложения
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">событие</param>
        private void BtnExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        /// <summary>
        /// Обработчик события кнопки входа и алгоритм дальнейшей авторизации
        /// </summary>
        /// <param name="sender">Отправитель</param>
        /// <param name="e">событие</param>
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
        /// <summary>
        /// Обрабатывает строку в кодировку MD5
        /// </summary>
        /// <param name="input">строка, которую необходимо захэшировать</param>
        /// <returns>захэшированная строка </returns>
        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// Обработчик закрытия окна
        /// </summary>
        /// <param name="sender">Отправитель</param>
        /// <param name="e">Событие</param>
        private void Window_OnClosing(object? sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}