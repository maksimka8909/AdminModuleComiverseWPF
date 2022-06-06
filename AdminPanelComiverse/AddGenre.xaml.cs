using System;
using System.ComponentModel;
using System.Windows;
using AdminPanelComiverse.Classes;
using ComicsApi.Models;
using RestSharp;

namespace AdminPanelComiverse;

public partial class AddGenre : Window
{
    private RestClient apiClient = ApiBuilder.GetInstance();
    /// <summary>
    /// Инициализация окна AddGenre 
    /// </summary>
    public AddGenre()
    {
        InitializeComponent();
    }
    /// <summary>
    /// Обработчик нажатия кнопки выхода на предыдущее окно
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void BtnBack_OnClick(object sender, RoutedEventArgs e)
    {
        this.Owner.Show();
        Hide();
    }
    /// <summary>
    /// Обработчик закрытия окна
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void Window_OnClosing(object? sender, CancelEventArgs e)
    {
        this.Owner.Show();
        Hide();
    }
    /// <summary>
    /// Обработчик кнопки добавления/изменения объекта типа жанр
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void BtnCreateAdd_OnClick(object sender, RoutedEventArgs e)
    {
        if (tbGenreName.Text.Trim().Length==0)
        {
            MessageBox.Show("Заполните все поля");
        }
        else
        {
            if (btnCreateAdd.Content.ToString() == "Создать")
            {
                var response = apiClient.Post<MassageClass>(new RestRequest("genre")
                    .AddBody( new Genre(){Name = tbGenreName.Text}));
                if (response.key == "EXIST")
                {
                    MessageBox.Show("Данный жанр уже внесен в базу данных");
                }
                else
                {
                    tbGenreName.Text = "";
                    MessageBox.Show("Жанр успешно внесен в базу данных");
                } 
            }
            else
            {
                var response = apiClient.Post<MassageClass>(new RestRequest("genre/update")
                    .AddBody( new Genre(){Id = Convert.ToInt32(tbGenreName.Tag),Name = tbGenreName.Text}));
                if (response.key == "EXIST")
                {
                    MessageBox.Show("Данный жанр уже внесен в базу данных");
                }
                else
                {
                    MessageBox.Show("Жанр успешно обновлен");
                } 
            }
        }
    }
}