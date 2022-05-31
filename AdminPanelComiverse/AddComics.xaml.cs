using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using AdminPanelComiverse.Classes;
using ComicsApi.Classes;
using ComicsApi.Models;
using Microsoft.Win32;
using RestSharp;

namespace AdminPanelComiverse;

public partial class AddComics : Window
{
    private RestClient apiClient = ApiBuilder.GetInstance();
    public string author { get; set; }
    public string editor { get; set; }
    /// <summary>
    /// Инициализация окна AddComics
    /// </summary>
    public AddComics()
    {
        InitializeComponent();
        btnCreateAdd.IsEnabled = false;
        
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
    /// Обработчик кнопки добавления/изменения объекта типа комикс
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void BtnCreateAdd_OnClick(object sender, RoutedEventArgs e)
    {
        if (btnCreateAdd.Content == "Создать")
        {
            if (tbPath.Text.Trim().Length == 0 
                || tbComicsDescription.Text.Trim().Length == 0
                || tbComicsName.Text.Trim().Length == 0
                || cbComicsAuthor.Text.Trim().Length == 0
                || cbComicsEditor.Text.Trim().Length == 0
                || dpComicsIssue.Text.Trim().Length == 0)
            {
                MessageBox.Show("Заполните все поля и выберите изображение");
            }
            else
            {
                ComboBoxItem comboBoxItemEditor = (ComboBoxItem)cbComicsEditor.SelectedItem;
                ComboBoxItem comboBoxItemAuthor = (ComboBoxItem)cbComicsAuthor.SelectedItem;
                var response = apiClient.Post<MassageClass>(new RestRequest("Comic")
                    .AddParameter("name",tbComicsName.Text)
                    .AddParameter("date",dpComicsIssue.Text)
                    .AddParameter("idAuthor",comboBoxItemAuthor.Tag.ToString())
                    .AddParameter("description",tbComicsDescription.Text)
                    .AddParameter("idEditor",comboBoxItemEditor.Tag.ToString())
                    .AddFile("cover",tbPath.Text));
                if (response.key == "EXIST")
                {
                    MessageBox.Show("Данный комикс уже внесен в базу данных");
                }
                else
                {
                    tbComicsName.Text = "";
                    dpComicsIssue.Text = "";
                    tbComicsDescription.Text = "";
                    tbPath.Text = "";
                    MessageBox.Show("Комикс успешно загружен");
                }
            }
        }
        else
        {
            if (btnCreateAdd.Content == "Изменить")
            {
                if (tbComicsDescription.Text.Trim().Length == 0
                    || tbComicsName.Text.Trim().Length == 0
                    || cbComicsAuthor.Text.Trim().Length == 0
                    || cbComicsEditor.Text.Trim().Length == 0
                    || dpComicsIssue.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Заполните все поля");
                }
                else
                {
                    ComboBoxItem comboBoxItemEditor = (ComboBoxItem)cbComicsEditor.SelectedItem;
                    ComboBoxItem comboBoxItemAuthor = (ComboBoxItem)cbComicsAuthor.SelectedItem;
                    var response = apiClient.Post<MassageClass>(new RestRequest($"Comic/Update")
                        .AddBody(new ComicsData()
                        {
                            Id = Convert.ToInt32(tbComicsName.Tag), Name = tbComicsName.Text,
                            Description = tbComicsDescription.Text, idAuthor = Convert.ToInt32(comboBoxItemAuthor.Tag),
                            idEditor = Convert.ToInt32(comboBoxItemEditor.Tag), dateOfIssue = dpComicsIssue.Text
                        }));
                    if (response.key == "EXIST")
                    {
                        MessageBox.Show("Данный комикс уже внесен в базу данных");
                    }
                    else
                    {
                        if (response.key == "OK")
                        {
                            MessageBox.Show("Данные об комиксе успешно обновлены");
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Обработчик события завершения загрузки окна и заполнение элементов Combobox
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Слбытие</param>
    private void Window_OnLoaded(object sender, RoutedEventArgs e)
    {
        var responseEditor = apiClient.Get<List<EditorData>>(new RestRequest("Editor"));
        var responseAuthor = apiClient.Get<List<Author>>(new RestRequest("Author"));
        if (responseEditor == null || responseEditor.Count == 0 || responseAuthor == null || responseAuthor.Count == 0)
        {
            MessageBox.Show("Издатели или авторы не найдены, повторите попытку или добавьте записи");
            this.Owner.Show();
            Hide();
        }
        else
        {
            btnCreateAdd.IsEnabled = true;
            for (int i = 0; i < responseEditor.Count; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = responseEditor[i].Name;
                item.Tag = responseEditor[i].Id;
                if (editor == responseEditor[i].Name) 
                {
                    item.IsSelected = true;
                }
                cbComicsEditor.Items.Add(item);
                
            }
            for (int i = 0; i < responseAuthor.Count; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = responseAuthor[i].Name + " " + responseAuthor[i].Surname;
                item.Tag = responseAuthor[i].Id;
                if (author == responseAuthor[i].Name + " " + responseAuthor[i].Surname)
                { 
                    item.IsSelected = true;
                }
                cbComicsAuthor.Items.Add(item);
            }
        }
    }
}