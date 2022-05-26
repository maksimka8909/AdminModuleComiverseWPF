using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AdminPanelComiverse.Classes;
using Microsoft.Win32;
using RestSharp;

namespace AdminPanelComiverse.Models;

public partial class AddIssue : Window
{
    private RestClient apiClient = ApiBuilder.GetInstance();
    public AddIssue()
    {
        InitializeComponent();
        btnCreateAdd.IsEnabled = false;
        dgIssues.Visibility = Visibility.Collapsed;
        var response = apiClient.Get<List<ComicsClass>>(new RestRequest("Comic"));
        if (response == null || response.Count == 0)
        {
            MessageBox.Show("Комиксы не найдены, повторите попытку или добавьте комикс");
            this.Owner.Show();
            Hide();
        }
        else
        {
            btnCreateAdd.IsEnabled = true;
            for (int i = 0; i < response.Count; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = response[i].name;
                item.Tag = response[i].id;
                cbComicsName.Items.Add(item);
            }
        }
    }

    private void BtnBack_OnClick(object sender, RoutedEventArgs e)
    {
        this.Owner.Show();
        Hide();
    }

    private void Window_OnClosing(object? sender, CancelEventArgs e)
    {
        this.Owner.Show();
        Hide();
    }

    private void BtnFindImage_OnClick(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Files|*.cbr;";
        if (openFileDialog.ShowDialog() == true)
        {
            tbPath.Text = openFileDialog.FileName;
        }
    }

    private void BtnCreateAdd_OnClick(object sender, RoutedEventArgs e)
    {
        if (cbComicsName.SelectedItem == null || tbPath.Text.Trim().Length == 0 || tbIssueName.Text.Trim().Length==0 || tbIssueNumber.Text.Trim().Length == 0)
        {
            MessageBox.Show("Заполните все поля и выберите изображение");
        }
        else
        {
            if (tbIssueNumber.Text.Length > 5)
            {
                MessageBox.Show("Номер выпуска не должен привышать 99999");
            }
            else
            {
                var fileParameter = FileParameter.FromFile(tbPath.Text);
                ComboBoxItem comboBoxItem = (ComboBoxItem)cbComicsName.SelectedItem;
                var response = apiClient.Get<MassageClass>(new RestRequest("Issue/CheckIssue")
                    .AddParameter("issueNumber", Convert.ToInt32(tbIssueNumber.Text))
                    .AddParameter("comicsId", Convert.ToInt32(comboBoxItem.Tag)));
                if (response.key == "OK")
                {
                    var request = new RestRequest("download/addfile");
                    request.Method = Method.Post;
                    request.AddHeader("Content-Type","multipart/form-data");
                    request.AddParameter("comicsId",comboBoxItem.Tag.ToString());
                    request.AddParameter("issueName",tbIssueName.Text);
                    request.AddFile("file",fileParameter.GetFile,fileParameter.FileName);
                    apiClient.Execute(request);
                    tbPath.Text = "";
                    tbIssueName.Text = "";
                    ComboBoxItem typeItem = (ComboBoxItem)cbComicsName.SelectedItem;
                    var responseIssue = apiClient.Get<List<IssueClass>>(new RestRequest("Comic/GetComicsIssue")
                        .AddParameter("idComics", Convert.ToInt32(typeItem.Tag)));
                    dgIssues.ItemsSource = responseIssue;
                    MessageBox.Show("Выпуск успешно загружен");
                }
                else
                {
                    MessageBox.Show("Данный номер комикса уже загружен");
                }
            }
            
        }
    }

    private void TbIssueNumber_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (!Char.IsDigit(e.Text, 0))
        {
            e.Handled = true;
        }
    }

    private void CbComicsName_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        dgIssues.Visibility = Visibility.Visible;
        ComboBoxItem typeItem = (ComboBoxItem)cbComicsName.SelectedItem;
        var response = apiClient.Get<List<IssueClass>>(new RestRequest("Comic/GetComicsIssue")
            .AddParameter("idComics", Convert.ToInt32(typeItem.Tag)));
        dgIssues.ItemsSource = response;
    }
}