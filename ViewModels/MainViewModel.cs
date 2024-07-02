using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using PacManWpfApp.Helpers;
using PacManWpfApp.Interfaces;
using PacManWpfApp.Models;
using System.Text.Json;
using PacManWpfApp.Views;

namespace PacManWpfApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IGameService _gameService;
        private bool _isPaused;
        private bool _isMenuOpen;
        private Level _selectedLevel;
        private readonly string _recordsFilePath = "game_records.json";
        private DateTime _startTime;

        public MainViewModel(IGameService gameService)
        {
            int len = 50;
            _startTime = DateTime.Now;
            _gameService = gameService;
            Levels = new List<Level>
            {
                new Level
                {
                    Name = "Small",
                    Ghosts = new List<Ghost>
                    {
                        new Ghost { X = 180, Y = 200, Speed = 3, Direction = Direction.Left },
                        new Ghost { X = 200, Y = 200, Speed = 3, Direction = Direction.Right },
                        new Ghost { X = 220, Y = 200, Speed = 3, Direction = Direction.Up },
                        new Ghost { X = 240, Y = 200, Speed = 3, Direction = Direction.Down }
                    },
                    Pellets = new List<Pellet>
                    {
                        new Pellet { X = 15, Y = 65 },
                        new Pellet { X = 15, Y = 85 },
                        new Pellet { X = 15, Y = 105 },
                        new Pellet { X = 15, Y = 125 },

                        new Pellet { X = 35, Y = 65 },
                        new Pellet { X = 55, Y = 65 },
                        new Pellet { X = 75, Y = 65 },
                        new Pellet { X = 95, Y = 65 },
                        new Pellet { X = 115, Y = 65 },
                        new Pellet { X = 135, Y = 65 },
                        new Pellet { X = 155, Y = 65 },
                        new Pellet { X = 175, Y = 65 },
                        new Pellet { X = 195, Y = 65 },
                        new Pellet { X = 215, Y = 65 },
                        new Pellet { X = 235, Y = 65 },
                        new Pellet { X = 255, Y = 65 },
                        new Pellet { X = 275, Y = 65 },
                        new Pellet { X = 295, Y = 65 },
                        new Pellet { X = 315, Y = 65 },
                        new Pellet { X = 335, Y = 65 },
                        new Pellet { X = 355, Y = 65 },
                        new Pellet { X = 375, Y = 65 },

                        new Pellet { X = 35, Y = 135 },
                        new Pellet { X = 55, Y = 135 },
                        new Pellet { X = 75, Y = 135 },
                        new Pellet { X = 95, Y = 135 },
                        new Pellet { X = 115, Y = 135 },


                        new Pellet { X = 385, Y = 85 },
                        new Pellet { X = 385, Y = 105 },
                        new Pellet { X = 385, Y = 125 },


                        new Pellet { X = 20, Y = 380 },
                        new Pellet { X = 380, Y = 380 },
                        // Add more pellets similarly...
                    },
                    Walls = new List<Wall>
                    {
                        // Outer boundary walls
                        new Wall { X = 0, Y = 50, Width = 400, Height = 10 },
                        new Wall { X = 0, Y = len, Width = 10, Height = 400 },
                        new Wall { X = 400, Y = len, Width = 10, Height = 400 },
                        new Wall { X = 0, Y = 400+len, Width = 410, Height = 10 },

                        // top-left
                        new Wall { X = 75, Y = 80, Width = 110, Height = 10 },
                        new Wall { X = 130, Y = 80, Width = 10, Height = 80 },

                        new Wall { X = 225, Y = 80, Width = 100, Height = 10 },
                        new Wall { X = 270, Y = 80, Width = 10, Height = 80 },

                        new Wall { X = 0, Y = 150, Width = 105, Height = 10 },

                        new Wall { X = 30, Y = 110, Width = 75, Height = 20 },
                        new Wall { X = 30, Y = 80, Width = 20, Height = 45 },

                        // top-right
                        new Wall { X = 310, Y = 150, Width = 100, Height = 10 },

                        new Wall { X = 310, Y = 110, Width = 65, Height = 20 },
                        new Wall { X = 355, Y = 80, Width = 20, Height = 45 },

                        // top-center
                        new Wall { X = 165, Y = 115, Width = 80, Height = 45 },

                        // center-left
                        new Wall { X = 0, Y = 250, Width = 100, Height = 10 },
                        new Wall { X = 90, Y = 185, Width = 10, Height = 70 },
                        new Wall { X = 35, Y = 185, Width = 30, Height = 40 },

                        // center-right
                        new Wall { X = 310, Y = 250, Width = 100, Height = 10 },
                        new Wall { X = 310, Y = 185, Width = 10, Height = 70 },
                        new Wall { X = 345, Y = 185, Width = 30, Height = 40 },

                        // center-center
                        new Wall { X = 130, Y = 190, Width = 150, Height = 10 },                    
                    
                        // center-center-spawn-enemy
                        new Wall { X = 130, Y = 275, Width = 150, Height = 10 },
                        new Wall { X = 130, Y = 235, Width = 10, Height = 40 },
                        new Wall { X = 270, Y = 235, Width = 10, Height = 40 },
                        new Wall { X = 130, Y = 225, Width = 60, Height = 10 },
                        new Wall { X = 220, Y = 225, Width = 60, Height = 10 },

                        // buttom-left
                        new Wall { X = 0, Y = 290, Width = 100, Height = 10 },
                        new Wall { X = 90, Y = 290, Width = 10, Height = 80 },
                        new Wall { X = 40, Y = 325, Width = 20, Height = 75 },
                        new Wall { X = 40, Y = 400, Width = 150, Height = 20 },


                        // buttom-center-left
                        new Wall { X = 130, Y = 315, Width = 10, Height = 55 },
                        new Wall { X = 180, Y = 315, Width = 10, Height = 55 },
                        new Wall { X = 130, Y = 360, Width = 50, Height = 10 },

                        // buttom-center-right
                        new Wall { X = 270, Y = 315, Width = 10, Height = 55 },
                        new Wall { X = 220, Y = 315, Width = 10, Height = 55 },
                        new Wall { X = 230, Y = 360, Width = 50, Height = 10 },

                        // buttom-right
                        new Wall { X = 310, Y = 290, Width = 100, Height = 10 },
                        new Wall { X = 310, Y = 290, Width = 10, Height = 80 },
                        new Wall { X = 350, Y = 325, Width = 20, Height = 75 },
                        new Wall { X = 220, Y = 400, Width = 150, Height = 20 },
                    }
                },
                new Level
                {
                    Name = "Medium",
                    Ghosts = new List<Ghost>
                    {
                        new Ghost { X = 200, Y = 200, Speed = 3, Direction = Direction.Left },
                        new Ghost { X = 250, Y = 250, Speed = 3, Direction = Direction.Right }
                    },
                    Pellets = new List<Pellet>
                    {
                        new Pellet { X = 50, Y = 50 },
                        new Pellet { X = 100, Y = 100 },
                        new Pellet { X = 150, Y = 150 },
                        new Pellet { X = 200, Y = 200 }
                    },
                    Walls = new List<Wall>
                    {
                        new Wall { X = 0, Y = 0, Width = 300, Height = 20 },
                        new Wall { X = 0, Y = 0, Width = 20, Height = 300 },
                        new Wall { X = 280, Y = 0, Width = 20, Height = 300 },
                        new Wall { X = 0, Y = 280, Width = 300, Height = 20 },
                        new Wall { X = 100, Y = 100, Width = 100, Height = 20 },
                        new Wall { X = 100, Y = 120, Width = 20, Height = 100 }
                    }
                },
                new Level
                {
                    Name = "Large",
                    Ghosts = new List<Ghost>
                    {
                        new Ghost { X = 200, Y = 200, Speed = 3, Direction = Direction.Left },
                        new Ghost { X = 250, Y = 250, Speed = 3, Direction = Direction.Right },
                        new Ghost { X = 300, Y = 300, Speed = 3, Direction = Direction.Up }
                    },
                    Pellets = new List<Pellet>
                    {
                        new Pellet { X = 150, Y = 350 },
                        new Pellet { X = 250, Y = 450 },
                        new Pellet { X = 350, Y = 550 },
                        new Pellet { X = 150, Y = 450 },
                        new Pellet { X = 300, Y = 300 }
                    },
                    Walls = new List<Wall>
                    {
                        new Wall { X = 0, Y = 60, Width = 500, Height = 20 },
                        new Wall { X = 0, Y = 540, Width = 500, Height = 20 }
                    }
                }
            };

            SelectedLevel = Levels.First();

            GameTimer = new DispatcherTimer();
            GameTimer.Interval = TimeSpan.FromMilliseconds(20);
            GameTimer.Tick += (sender, args) => UpdateGame();
            GameTimer.Start();

            KeyDownCommand = new RelayCommand<KeyEventArgs>(OnKeyDown);
            StartGameCommand = new RelayCommand(StartGame);
            Records = LoadRecords();

            // commands
            OpenRecordsCommand = new RelayCommand(OpenRecords);
            SaveRecordCommand = new RelayCommand(SaveRecord);
            // Initialize game with default level
            StartGame();
        }

        public DispatcherTimer GameTimer { get; }
        public ICommand KeyDownCommand { get; }
        public ICommand StartGameCommand { get; }

        public List<Level> Levels { get; }

        public Level SelectedLevel
        {
            get => _selectedLevel;
            set
            {
                _selectedLevel = value;
                OnPropertyChanged(nameof(SelectedLevel));
            }
        }

        private void StartGame()
        {
            _gameService.InitializeGame(SelectedLevel);
            OnPropertyChanged(nameof(PacMan));
            OnPropertyChanged(nameof(Ghosts));
            OnPropertyChanged(nameof(Pellets));
            OnPropertyChanged(nameof(Walls));
            OnPropertyChanged(nameof(Lives));
            OnPropertyChanged(nameof(PalletsNumber));
        }

        private void UpdateGame()
        {
            if (!_isPaused && !_isMenuOpen)
            {
                _gameService.UpdateGame();
                OnPropertyChanged(nameof(PacMan));
                OnPropertyChanged(nameof(Ghosts));
                OnPropertyChanged(nameof(Pellets));
                OnPropertyChanged(nameof(Walls));
                OnPropertyChanged(nameof(Lives));
                OnPropertyChanged(nameof(PalletsNumber));
                if (PacMan.Lives <= 0 || PalletsNumber >= 10)
                {
                    GameOver?.Invoke();
                }
            }
        }
        public event Action GameOver;

        private void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    _gameService.HandleInput(Direction.Up);
                    break;
                case Key.Down:
                    _gameService.HandleInput(Direction.Down);
                    break;
                case Key.Left:
                    _gameService.HandleInput(Direction.Left);
                    break;
                case Key.Right:
                    _gameService.HandleInput(Direction.Right);
                    break;
                case Key.Space:
                    TogglePause();
                    break;
                case Key.Escape:
                    ToggleMenu();
                    break;

            }
        }

        private void TogglePause()
        {
            _isPaused = !_isPaused;
        }

        private void ToggleMenu()
        {
            _isMenuOpen = !_isMenuOpen;
        }

        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                _isPaused = value;
                OnPropertyChanged(nameof(IsPaused));
            }
        }

        public bool IsMenuOpen
        {
            get => _isMenuOpen;
            set
            {
                _isMenuOpen = value;
                OnPropertyChanged(nameof(IsMenuOpen));
            }
        }

        // Properties and collections
        public ObservableCollection<GameRecord> Records { get; set; }

        public ICommand OpenRecordsCommand { get; }
        public ICommand SaveRecordCommand { get; }

        private void OpenRecords()
        {
            var recordsWindow = new RecordsWindow(Records);
            recordsWindow.ShowDialog();
        }

        private void SaveRecord()
        {
            TimeSpan gameDuration = DateTime.Now - _startTime; // Calculate game duration
            var saveRecordWindow = new SaveRecordWindow(Records, PalletsNumber, gameDuration);
            saveRecordWindow.Show();

        }

        private ObservableCollection<GameRecord> LoadRecords()
        {
            if (!File.Exists(_recordsFilePath))
                return new ObservableCollection<GameRecord>();

            var json = File.ReadAllText(_recordsFilePath);
            return JsonSerializer.Deserialize<ObservableCollection<GameRecord>>(json);
        }

        public PacMan PacMan => _gameService.PacMan;
        public List<Ghost> Ghosts => _gameService.Ghosts;
        public List<Pellet> Pellets => _gameService.Pellets;
        public List<Wall> Walls => _gameService.Walls;
        public int Lives => _gameService.Lives;
        public int PalletsNumber => _gameService.PalletsNumber;
    }

}
