using System;
using System.ComponentModel;
using System.Windows;
using AdminPanelComiverse.Classes;
using Microsoft.Win32;
using RestSharp;

namespace AdminPanelComiverse;

public partial class AddEditor : Window
{
    private RestClient apiClient = ApiBuilder.GetInstance();
    public AddEditor()
    {
        InitializeComponent();
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
        openFileDialog.Filter = "Files|*.jpg;*.jpeg;*.png";
        if (openFileDialog.ShowDialog() == true)
        {
            tbPath.Text = openFileDialog.FileName;
        }
    }

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
                    .AddBody(new {id = Convert.ToInt32(tbEditorName.Tag), name = tbEditorName.Name}));
                if (response1.key == "EXIST")
                {
                    MessageBox.Show("Данный издатель уже внесен в базу данных");
                }
                else
                {
                    MessageBox.Show(response1.key);
                }
            }
        }
        
    }
}