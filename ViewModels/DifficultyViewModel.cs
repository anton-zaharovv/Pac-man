using PacManWpfApp.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PacManWpfApp.ViewModels
{
    public class DifficultyViewModel : ViewModelBase
    {
        private string _selectedLevel;
        public ObservableCollection<string> Levels { get; set; }
        public string SelectedLevel
        {
            get => _selectedLevel;
            set
            {
                _selectedLevel = value;
                OnPropertyChanged(nameof(SelectedLevel));
            }
        }

        public ICommand StartGameCommand { get; }

        public DifficultyViewModel()
        {
            Levels = new ObservableCollection<string> { "Easy", "Medium", "Hard" };
            SelectedLevel = Levels[0];
            StartGameCommand = new RelayCommand(StartGame);
        }

        private void StartGame()
        {
            // Handle starting the game with the selected difficulty level
        }
    }
}
