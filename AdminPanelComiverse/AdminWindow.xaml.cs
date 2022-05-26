using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using AdminPanelComiverse.Classes;
using AdminPanelComiverse.Models;
using ComicsApi.Classes;
using RestSharp;

namespace AdminPanelComiverse;

public partial class AdminWindow : Window
{
    private RestClient apiClient = ApiBuilder.GetInstance();
    public AdminWindow()        
    {
        InitializeComponent();
        var response = apiClient.Get<List<UserClass>>(new RestRequest("User"));
        dgUsers.ItemsSource = response;
    }

    private void Window_OnClosing(object? sender, CancelEventArgs e)
    {
        Application.Current.Shutdown();
    }
    
    private void BtnBlockUser_OnClick(object sender, RoutedEventArgs e)
    {
        if (dgUsers.SelectedItem == null)
        {
            MessageBox.Show("Пользователь не выбран");
        }
        else
        {
            apiClient.Get(new RestRequest("User/ChangeState")
                .AddParameter("id",(dgUsers.SelectedItem as UserClass).id));
            var response = apiClient.Get<List<UserClass>>(new RestRequest("User"));
            dgUsers.ItemsSource = response;
        }
    }

    private void BtnSearch_OnClick(object sender, RoutedEventArgs e)
    {
        if (tbSearch.Text.Trim().Length == 0)
        {
            var response = apiClient.Get<List<UserClass>>(new RestRequest("User"));
            dgUsers.ItemsSource = response;
        }
        else
        {
            var response = apiClient.Get<List<UserClass>>(new RestRequest("User/Search")
                .AddParameter("request",tbSearch.Text.Trim()));
            dgUsers.ItemsSource = response;
        }
    }

    private void BtnExit_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void CbList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        
        ComboBoxItem typeItem = (ComboBoxItem)cbList.SelectedItem;
        switch (typeItem.Content.ToString())
        {
            case "Комиксы":
                var response1 = apiClient.Get<List<ComicsClass>>(new RestRequest("Comic"));
                dgData.ItemsSource = response1;
                break;
            case "Авторы":
                var response2 = apiClient.Get<List<AuthorData>>(new RestRequest("Author"));
                dgData.ItemsSource = response2;
                break;
            case "Издатели":
                var response3 = apiClient.Get<List<EditorData>>(new RestRequest("Editor"));
                dgData.ItemsSource = response3;
                break;
            case "Жанры":
                var response4 = apiClient.Get<List<GenreData>>(new RestRequest("Genre"));
                dgData.ItemsSource = response4;
                break;
            default:
                var response5 = apiClient.Get<List<ComicsClass>>(new RestRequest("Comic"));
                dgData.ItemsSource = response5;
                break;
        }
    }

    private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
    {
        ComboBoxItem typeItem = (ComboBoxItem)cbList.SelectedItem;
        switch (typeItem.Content.ToString())
        {
            case "Комиксы":
                AddComics addComics = new AddComics();
                addComics.Owner = this;
                addComics.Show();
                this.Hide();
                break;
            case "Авторы":
                AddAuthor addAuthor = new AddAuthor();
                addAuthor.Owner = this;
                addAuthor.Show();
                this.Hide();
                break;
            case "Издатели":
                AddEditor addEditor = new AddEditor();
                addEditor.Owner = this;
                addEditor.Show();
                this.Hide();
                break;
            case "Жанры":
                AddGenre addGenre = new AddGenre();
                addGenre.Owner = this;
                addGenre.Show();
                this.Hide();
                break;
            default:
                MessageBox.Show("Ошибка, выберите категорию, в которую хотите добавить запись");
                break;
        }
    }

    private void BtnPullIssue_OnClick(object sender, RoutedEventArgs e)
    {
        AddIssue addIssue = new AddIssue();
        addIssue.Owner = this;
        addIssue.Show();
        this.Hide();
    }

    private void BtnUpdate_OnClick(object sender, RoutedEventArgs e)
    {
        ComboBoxItem typeItem = (ComboBoxItem)cbList.SelectedItem;
        if (dgData.SelectedItem == null)
        {
            MessageBox.Show("Ошибка, выберите запись, которую хотите изменить");
        }
        else
        {
            switch (typeItem.Content.ToString())
            {
                case "Комиксы":
                    MessageBox.Show((dgData.SelectedItem as ComicsClass)?.name);
                    AddComics addComics = new AddComics();
                    addComics.btnCreateAdd.Content = "Изменить";
                    addComics.Owner = this;
                    addComics.Show();
                    this.Hide();
                    break;
                case "Авторы":
                    MessageBox.Show((dgData.SelectedItem as AuthorData)?.Id.ToString());
                    AddAuthor addAuthor = new AddAuthor();
                    addAuthor.Owner = this;
                    addAuthor.btnCreateAdd.Content = "Изменить";
                    addAuthor.Show();
                    this.Hide();
                    break;
                case "Издатели":
                    AddEditor addEditor = new AddEditor();
                    addEditor.tbEditorName.Tag = (dgData.SelectedItem as EditorData)?.Id;
                    addEditor.tbEditorName.Text = (dgData.SelectedItem as EditorData)?.Name;
                    addEditor.btnCreateAdd.Content = "Изменить";
                    addEditor.lLogo.Visibility = Visibility.Hidden;
                    addEditor.tbPath.Visibility = Visibility.Hidden;
                    addEditor.btnFindImage.Visibility = Visibility.Hidden;
                    addEditor.Owner = this;
                    addEditor.Show();
                    this.Hide();
                    break;
                case "Жанры":
                    MessageBox.Show((dgData.SelectedItem as GenreData)?.name);
                    AddGenre addGenre = new AddGenre();
                    addGenre.btnCreateAdd.Content = "Изменить";
                    addGenre.tbGenreName.Text = (dgData.SelectedItem as GenreData)?.name;
                    addGenre.tbGenreName.Tag = (dgData.SelectedItem as GenreData)?.id;
                    addGenre.Owner = this;
                    addGenre.Show();
                    this.Hide();
                    break;
                case null:
                    MessageBox.Show("Ошибка, выберите категорию, в которую хотите добавить запись");
                    break;
                default:
                    MessageBox.Show("Ошибка, выберите категорию, в которую хотите добавить запись");
                    break;
            }
        }
        

    }
}