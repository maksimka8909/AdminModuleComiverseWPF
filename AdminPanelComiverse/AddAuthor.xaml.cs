using System.ComponentModel;
using System.Windows;
using AdminPanelComiverse.Classes;
using Microsoft.Win32;
using RestSharp;

namespace AdminPanelComiverse;

public partial class AddAuthor : Window
{
    private RestClient apiClient = ApiBuilder.GetInstance();
    public AddAuthor()
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
                .AddParameter("name",tbAuthorName.Text)
                .AddParameter("surname",tbAuthorSurname.Text)
                .AddParameter("middleName",tbAuthorMiddlename.Text)
                .AddParameter("description",tbAuthorDescription.Text)
                .AddParameter("birthday",dpAuthorBirthday.Text)
                .AddFile("image",tbPath.Text));
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
}