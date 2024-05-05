using System.Collections.ObjectModel;
using Allegro_Price_Detector.Models;

namespace Allegro_Price_Detector.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Auction> Auctions { get; } = new ObservableCollection<Auction>();

    }
}