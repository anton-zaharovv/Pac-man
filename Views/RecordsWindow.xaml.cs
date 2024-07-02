using PacManWpfApp.Models;
using System.Collections.ObjectModel;
using System.Windows;


namespace PacManWpfApp.Views
{
    /// <summary>
    /// Interaction logic for RecordsWindow.xaml
    /// </summary>
    public partial class RecordsWindow : Window
    {
        public RecordsWindow(ObservableCollection<GameRecord> records)
        {
            InitializeComponent();
            DataContext = this;
            Records = records;

        }

        public ObservableCollection<GameRecord> Records { get; }
    }
}
