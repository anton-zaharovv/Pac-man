using PacManWpfApp.Services;
using PacManWpfApp.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PacManWpfApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private DateTime _startTime;
        private bool _isPaused;
        private bool _isMenuOpen;
        private bool _isStartGame = true;
        private bool _isBackgroundSoundPaused;
        private MediaPlayer _gameOverSound;
        private MediaPlayer _victorySound;
        private MediaPlayer _backgroundSound;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(new GameService());
            this.KeyDown += (sender, e) => ((MainViewModel)DataContext).KeyDownCommand.Execute(e);

            CompositionTarget.Rendering += GameCanvas_OnRender;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1);
            _timer.Tick += Timer_Tick;
            
            _gameOverSound = new MediaPlayer();
            _gameOverSound.Open(new Uri("G:\\.NET freelance\\PacManWpfApp\\bin\\Debug\\Data\\game_over.wav"));
            //_gameOverSound.Play();


            _victorySound = new MediaPlayer();
            _victorySound.Open(new Uri("G:\\.NET freelance\\PacManWpfApp\\bin\\Debug\\Data\\victory.mp3"));

            _backgroundSound = new MediaPlayer();
            _backgroundSound.Open(new Uri("G:\\.NET freelance\\PacManWpfApp\\bin\\Debug\\Data\\background_music.mp3"));
            _isBackgroundSoundPaused = false;
            StartTimer();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!_isPaused && !_isMenuOpen)
            {
                TimeSpan elapsed = DateTime.Now - _startTime;
                TimerTextBlock.Text = string.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                                                    elapsed.Hours,
                                                    elapsed.Minutes,
                                                    elapsed.Seconds,
                                                    elapsed.Milliseconds);
            }
        }

        private void StartTimer()
        {
            _startTime = DateTime.Now;
            _timer.Start();
        }

        private void ResetTimer()
        {
            _timer.Stop();
            _startTime = DateTime.Now;
            TimerTextBlock.Text = "00:00:00.000";
            _timer.Start();
        }

        private void GameCanvas_OnRender(object sender, EventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            GameCanvas.Children.Clear();

            // Draw Pac-Man
            Ellipse pacManShape = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Yellow
            };
            Canvas.SetLeft(pacManShape, vm.PacMan.X);
            Canvas.SetTop(pacManShape, vm.PacMan.Y);
            GameCanvas.Children.Add(pacManShape);

            // Draw Ghosts
            foreach (var ghost in vm.Ghosts)
            {
                Ellipse ghostShape = new Ellipse
                {
                    Width = 20,
                    Height = 20,
                    Fill = Brushes.Red
                };
                Canvas.SetLeft(ghostShape, ghost.X);
                Canvas.SetTop(ghostShape, ghost.Y);
                GameCanvas.Children.Add(ghostShape);
            }

            // Draw Pellets
            foreach (var pellet in vm.Pellets)
            {
                if (!pellet.IsEaten)
                {
                    Ellipse pelletShape = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.White
                    };
                    Canvas.SetLeft(pelletShape, pellet.X);
                    Canvas.SetTop(pelletShape, pellet.Y);
                    GameCanvas.Children.Add(pelletShape);
                }
            }

            // Draw Walls
            foreach (var wall in vm.Walls)
            {
                Rectangle wallShape = new Rectangle
                {
                    Width = wall.Width,
                    Height = wall.Height,
                    Fill = Brushes.Blue
                };
                Canvas.SetLeft(wallShape, wall.X);
                Canvas.SetTop(wallShape, wall.Y);
                GameCanvas.Children.Add(wallShape);
            }

            // Display Lives
            TextBlock livesText = new TextBlock
            {
                Text = $"Lives: {vm.PacMan.Lives}",
                Foreground = Brushes.White,
                FontSize = 16
            };
            Canvas.SetLeft(livesText, 10);
            Canvas.SetTop(livesText, 10);
            GameCanvas.Children.Add(livesText);
            
            // Display Pellets
            TextBlock pelletText = new TextBlock
            {
                Text = $"Pellets: {vm.PalletsNumber + 50}",
                Foreground = Brushes.White,
                FontSize = 16
            };
            Canvas.SetLeft(pelletText, 180);
            Canvas.SetTop(pelletText, 10);
            GameCanvas.Children.Add(pelletText);
            if (!_isBackgroundSoundPaused)
            {
                _backgroundSound.Play();
            }
            
            // Check for game over condition
            if ((vm.PacMan.Lives <= 0 || vm.PalletsNumber >= 10) && _isStartGame == true)
            {
                _isStartGame = false;
                GameOver();
            }
        }

        private void ExitGameButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            _isMenuOpen = true;
            _timer.Stop();
            vm.IsMenuOpen = true;
        }

        private void CloseMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            _isMenuOpen = false;
            _timer.Start();
            vm.IsMenuOpen = false;
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            ResetTimer();
            _isStartGame = true;
            _isBackgroundSoundPaused = false;
        }


        private void GameOver()
        {
            try
            {
                var vm = (MainViewModel)DataContext;
                _isMenuOpen = true;
                _timer.Stop();
                vm.SaveRecordCommand.Execute(null);
                _isBackgroundSoundPaused = true;
                _backgroundSound.Stop();

                if (vm.PacMan.Lives <= 0)
                {
                    _gameOverSound.Play();

                    MessageBox.Show("Game Over!");
                }
                else if (vm.PalletsNumber >= 10)
                {
                    _victorySound.Play();

                    MessageBox.Show("Victory!");
                }


                vm.IsMenuOpen = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

