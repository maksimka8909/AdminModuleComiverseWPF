﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using AdminPanelComiverse.Classes;
using Microsoft.AspNetCore.Http;
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
        //Files|*.jpg;*.jpeg;*.png;
    }

    private void BtnCreateAdd_OnClick(object sender, RoutedEventArgs e)
    {
        if (cbComicsName.SelectedItem == null || tbPath.Text.Trim().Length == 0 || tbIssueName.Text.Trim().Length==0)
        {
            MessageBox.Show("Поля не заполнены");
        }
        else
        {
            var fileParameter = FileParameter.FromFile(tbPath.Text);
            ComboBoxItem comboBoxItem = (ComboBoxItem)cbComicsName.SelectedItem;
                var response = apiClient.Post(new RestRequest("download/addfile")
                    .AddFile("file",fileParameter.GetFile,fileParameter.FileName,fileParameter.ContentType)
                    .AddParameter("comicsId",Convert.ToInt32(comboBoxItem.Tag))
                    .AddParameter("issueName",tbIssueName.Text));
                MessageBox.Show(response.Content);


        }
    }
}