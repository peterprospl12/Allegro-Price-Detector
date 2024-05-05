using Allegro_Price_Detector.Models;
using Allegro_Price_Detector.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Allegro_Price_Detector.Views;

public partial class MainWindow : Window
{
    Client client = new Client();
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        LinkTextBox.IsVisible = true;
        // Get the link from the user. This is just an example, replace it with your actual code.
        string link = "http://example.com";

        // Get the ViewModel.
        var viewModel = (MainWindowViewModel)DataContext;

        // Add a new Auction to the Auctions collection.
        //viewModel.Auctions.Add(new Auction { Link = link });
    }
    
    private async void LinkTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            // Get the link from the TextBox.
            string link = LinkTextBox.Text;

            // Get the ViewModel.
            var viewModel = (MainWindowViewModel)DataContext;

            // Add a new Auction to the Auctions collection.
            var auction = new Auction(link);
            var result = await auction.GetAuctionPriceAsync(client);
            viewModel.Auctions.Add(auction);
            
            // Clear the TextBox and hide it.
            LinkTextBox.Text = string.Empty;
            LinkTextBox.IsVisible = false;
        }
    }
}