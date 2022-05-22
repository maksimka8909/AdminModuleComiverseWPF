﻿using System.ComponentModel;
using System.Windows;
using AdminPanelComiverse.Classes;
using ComicsApi.Models;
using RestSharp;

namespace AdminPanelComiverse;

public partial class AddGenre : Window
{
    private RestClient apiClient = ApiBuilder.GetInstance();
    public AddGenre()
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

    private void BtnCreateAdd_OnClick(object sender, RoutedEventArgs e)
    {
        if (tbGenreName.Text.Trim().Length==0)
        {
            MessageBox.Show("Заполните все поля");
        }
        else
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
    }
}