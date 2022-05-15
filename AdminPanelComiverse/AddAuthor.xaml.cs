using System.ComponentModel;
using System.Windows;

namespace AdminPanelComiverse;

public partial class AddAuthor : Window
{
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
}