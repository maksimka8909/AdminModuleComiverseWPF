using System;
using System.ComponentModel;
using System.Windows;
using AdminPanelComiverse.Classes;
using Microsoft.Win32;
using RestSharp;

namespace AdminPanelComiverse;

public partial class AddAuthor : Window
{
    private RestClient apiClient = ApiBuilder.GetInstance();
    /// <summary>
    /// Инициализация окна AddAuthor
    /// </summary>
    public AddAuthor()
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
    /// Обработчик нажатия кнопки поиска изображения
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void BtnFindImage_OnClick(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Files|*.jpg;*.jpeg;*.png";
        if (openFileDialog.ShowDialog() == true)
        {
            tbPath.Text = openFileDialog.FileName;
        }
    }
    
    /// <summary>
    /// Обработчик кнопки добавления/изменения объекта типа автор
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void BtnCreateAdd_OnClick(object sender, RoutedEventArgs e)
    {
        if (btnCreateAdd.Content.ToString() == "Создать")
        {
            if (tbPath.Text.Trim().Length == 0
                || tbAuthorDescription.Text.Trim().Length == 0
                || tbAuthorName.Text.Trim().Length == 0
                || tbAuthorSurname.Text.Trim().Length == 0
                || dpAuthorBirthday.Text.Trim().Length == 0)
            {
                MessageBox.Show("Заполните все поля и выберите изображение");
            }
            else
            {
                var response = apiClient.Post<MassageClass>(new RestRequest("Author")
                    .AddParameter("name", tbAuthorName.Text)
                    .AddParameter("surname", tbAuthorSurname.Text)
                    .AddParameter("middleName", tbAuthorMiddlename.Text)
                    .AddParameter("description", tbAuthorDescription.Text)
                    .AddParameter("birthday", dpAuthorBirthday.Text)
                    .AddFile("image", tbPath.Text));
                if (response.key == "EXIST")
                {
                    MessageBox.Show("Данный автор уже внесен в базу данных");
                }
                else
                {
                    tbAuthorName.Text = "";
                    tbAuthorSurname.Text = "";
                    tbAuthorMiddlename.Text = "";
                    tbAuthorDescription.Text = "";
                    dpAuthorBirthday.Text = "";
                    tbPath.Text = "";
                    MessageBox.Show("Автор успешно внесен в базу данных");
                }
            }
        }
        else
        {
            if (btnCreateAdd.Content.ToString() == "Изменить")
            {
                if (tbAuthorDescription.Text.Trim().Length == 0
                    || tbAuthorName.Text.Trim().Length == 0
                    || tbAuthorSurname.Text.Trim().Length == 0
                    || dpAuthorBirthday.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Заполните все поля");
                }
                else
                {
                    var response = apiClient.Post<MassageClass>(new RestRequest("Author/update")
                        .AddBody(new AuthorClass()
                        {
                            Id = Convert.ToInt32(tbAuthorName.Tag), Name = tbAuthorName.Text,
                            Surname = tbAuthorSurname.Text, MiddleName = tbAuthorMiddlename.Text,
                            Description = tbAuthorDescription.Text, Birthday = dpAuthorBirthday.Text
                        }));
                    if (response.key == "EXIST")
                    {
                        MessageBox.Show("Данный автор уже внесен в базу данных");
                    }
                    else
                    {
                        if (response.key == "OK")
                        {
                            MessageBox.Show("Данные об автору успешно обновлены");
                        }
                    }
                }
            }
        }
    }
}