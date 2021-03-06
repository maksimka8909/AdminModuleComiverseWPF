using System;
using System.ComponentModel;
using System.Windows;
using AdminPanelComiverse.Classes;
using ComicsApi.Classes;
using Microsoft.Win32;
using RestSharp;

namespace AdminPanelComiverse;

public partial class AddEditor : Window
{
    private RestClient apiClient = ApiBuilder.GetInstance();
    /// <summary>
    /// Инициализация окна AddEditor
    /// </summary>
    public AddEditor()
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
    /// Обработчик кнопки добавления/изменения объекта типа издатель
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void BtnCreateAdd_OnClick(object sender, RoutedEventArgs e)
    {
        if (btnCreateAdd.Content != "Изменить")
        {
            if (tbPath.Text.Trim().Length == 0 || tbEditorName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Заполните все поля и выберите изображение");
            }
            else
            {
                var response = apiClient.Post<MassageClass>(new RestRequest("Editor")
                    .AddParameter("name", tbEditorName.Text)
                    .AddFile("logo", tbPath.Text));
                if (response.key == "EXIST")
                {
                    MessageBox.Show("Данный издатель уже внесен в базу данных");
                }
                else
                {
                    tbEditorName.Text = "";
                    tbPath.Text = "";
                    MessageBox.Show("Издатель успешно внесен в базу данных");
                }
            }
        }
        else
        {
            if (tbEditorName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Заполните поле");
            }
            else
            {
                var response1 = apiClient.Post<MassageClass>(new RestRequest("editor/updateEditor")
                    .AddBody(new EditorData(){Id = Convert.ToInt32(tbEditorName.Tag), Name = tbEditorName.Text}));
                if (response1.key == "EXIST")
                {
                    MessageBox.Show("Данный издатель уже внесен в базу данных");
                }
                else
                {
                    if (response1.key == "OK")
                    {
                        MessageBox.Show("Данные об издателе успешно изменены");
                    }
                }
            }
        }
    }
}