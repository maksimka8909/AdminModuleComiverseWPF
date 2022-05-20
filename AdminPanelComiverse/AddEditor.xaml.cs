using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Win32;

namespace AdminPanelComiverse;

public partial class AddEditor : Window
{
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
        if (tbPath.Text.Trim().Length == 0 || tbGenreName.Text.Trim().Length == 0)
        {
            MessageBox.Show("Заполните все поля и выберите изображение");
        }
        else
        {
            
        }
    }
}