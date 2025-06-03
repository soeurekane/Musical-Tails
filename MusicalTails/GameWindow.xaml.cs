using Model.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MusicalTails
{
    public partial class GameWindow : Window
    {
        private readonly GameLogic _gameLogic;
        private readonly System.Windows.Threading.DispatcherTimer _gameTimer;
        private readonly System.Windows.Threading.DispatcherTimer _spawnTimer;
        private double _baseFallSpeed = 5.0;
        private bool _isClosed;
        private LongButton _currentLongButton = null;
        private Rectangle _currentLongButtonRect = null;

        public GameWindow(GameLogic gameLogic)
        {
            InitializeComponent();
            _gameLogic = gameLogic;
            _gameTimer = new System.Windows.Threading.DispatcherTimer();
            _spawnTimer = new System.Windows.Threading.DispatcherTimer();

            SetupGame();
        }

        private void SetupGame()
        {
            _gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            _gameTimer.Tick += GameLoop;
            _gameLogic.ScoreUpdated += UpdateScore;
            _gameLogic.GameEnded += OnGameEnded;

            this.PreviewMouseDown += HandleButtonPress;
            this.PreviewMouseUp += HandleButtonRelease;

            DrawLanes();
            _gameTimer.Start();
            StartSpawningButtons();
        }

        private void DrawLanes()
        {
            GameCanvas.Children.Clear();

            if (GameCanvas.ActualWidth <= 0 || _gameLogic.LaneCount <= 0)
                return;

            double laneWidth = GameCanvas.ActualWidth / _gameLogic.LaneCount;
            double laneRenderWidth = Math.Max(10, laneWidth - 10);

            for (int i = 0; i < _gameLogic.LaneCount; i++)
            {
                var lane = new Rectangle
                {
                    Width = laneRenderWidth,
                    Height = GameCanvas.ActualHeight,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1,
                    Opacity = 0.3
                };

                double left = (i * laneWidth) + (laneWidth - laneRenderWidth) / 2;
                Canvas.SetLeft(lane, left);
                GameCanvas.Children.Add(lane);
            }
        }

        private void StartSpawningButtons()
        {
            _spawnTimer.Tick += (s, e) =>
            {
                if (_gameTimer.IsEnabled)
                {
                    SpawnButton(_gameLogic.ButtonGenerator.GenerateRandomButton(_gameLogic.Score));
                    int delay = Math.Max(300, 1000 - (_gameLogic.Difficulty * 50));
                    _spawnTimer.Interval = TimeSpan.FromMilliseconds(delay);
                }
                else
                {
                    _spawnTimer.Stop();
                }
            };

            int initialDelay = Math.Max(300, 1000 - (_gameLogic.Difficulty * 50));
            _spawnTimer.Interval = TimeSpan.FromMilliseconds(initialDelay);
            _spawnTimer.Start();
        }

        private void SpawnButton(IMusicalButton button)
        {
            int width = 60;
            int height = button is LongButton ? 200 : 120;
            int lane = new Random().Next(_gameLogic.LaneCount);
            var tile = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = GetButtonBrush(button),
                Stroke = Brushes.White,
                StrokeThickness = 2,
                RadiusX = 5,
                RadiusY = 5,
                Tag = button,
                Opacity = 0.9
            };

            lane = new Random().Next(_gameLogic.Difficulty);
            double laneWidth = GameCanvas.ActualWidth / _gameLogic.Difficulty;
            double left = (lane * laneWidth) + (laneWidth - tile.Width) / 2;
            Canvas.SetLeft(tile, left);
            Canvas.SetTop(tile, -tile.Height);
            GameCanvas.Children.Add(tile);
        }

        private static Brush GetButtonBrush(IMusicalButton button)
        {
            Color baseColor = button switch
            {
                ShortButton => Color.FromRgb(127, 255, 212),  // розовый
                LongButton => Color.FromRgb(0, 0, 255),   // Синий
                DoubleTapButton => Color.FromRgb(238, 130, 238), // фиол
                TrapButton => Color.FromRgb(255, 100, 100),    // Красный
                _ => Colors.White
            };

            return new LinearGradientBrush
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop(baseColor, 0),
                    new GradientStop(Colors.Black, 1)
                }
            };
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (_gameLogic.IsGameOver) return;

            double fallSpeed = _baseFallSpeed * _gameLogic.ButtonGenerator.GetSpeedMultiplier(_gameLogic.Score);
            var toRemove = new List<UIElement>();

            foreach (UIElement child in GameCanvas.Children)
            {
                if (child is Rectangle rect && rect.Tag is IMusicalButton button)
                {
                    double newTop = Canvas.GetTop(rect) + fallSpeed;
                    Canvas.SetTop(rect, newTop);

                    if (newTop > GameCanvas.ActualHeight - HitZone.ActualHeight)
                    {
                        if (!button.IsPressed && button is not TrapButton)
                        {
                            if (button is LongButton longButton && longButton.WasAttempted)
                            {
                                // Не вызываем RegisterMissedButton
                            }
                            else
                            {
                                _gameLogic.RegisterMissedButton(button);
                                UpdateLivesDisplay(); 
                            }
                        }
                        toRemove.Add(rect);
                    }
                }
            }

            foreach (var element in toRemove)
            {
                GameCanvas.Children.Remove(element);
            }
        }

        private void HandleButtonPress(object sender, MouseButtonEventArgs e)
        {
            var hit = VisualTreeHelper.HitTest(GameCanvas, e.GetPosition(GameCanvas));
            if (hit?.VisualHit is Rectangle rect && rect.Tag is IMusicalButton button)
            {
                button.PlaySound();

                switch (button)
                {
                    case LongButton longButton:
                        _currentLongButton = longButton;
                        _currentLongButtonRect = rect;
                        longButton.StartPress();
                        rect.Fill = new SolidColorBrush(Colors.LightBlue); // Синий при начале нажатия
                        break;

                    case DoubleTapButton doubleTap:
                        if (doubleTap.RegisterTap())
                        {
                            _gameLogic.AddScore(doubleTap.ScoreValue);
                            rect.Fill = Brushes.Lime;
                            GameCanvas.Children.Remove(rect);
                        }
                        else
                        {
                            rect.Fill = Brushes.Yellow;
                        }
                        break;

                    case TrapButton trap:
                        _gameLogic.ProcessTrapButton(trap);
                        rect.Fill = Brushes.Red;
                        GameCanvas.Children.Remove(rect);
                        break;
                    default:
                        _gameLogic.AddScore(button.ScoreValue);
                        button.IsPressed = true;
                        rect.Fill = Brushes.Lime;
                        GameCanvas.Children.Remove(rect);
                        break;
                }
            }
        }

        private void HandleButtonRelease(object sender, MouseButtonEventArgs e)
        {
            if (_currentLongButton != null && _currentLongButtonRect != null)
            {
                bool success = _currentLongButton.EndPress();

                if (success)
                {
                    _gameLogic.AddScore(_currentLongButton.ScoreValue);
                    _currentLongButtonRect.Fill = Brushes.Lime;
                }
                else // Если не удержали
                {
                    _currentLongButtonRect.Fill = Brushes.Red;
                    _gameLogic.RegisterMissedButton(_currentLongButton);
                    UpdateLivesDisplay();
                }

                // Плавное исчезновение
                var anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.2));
                anim.Completed += (s, _) => GameCanvas.Children.Remove(_currentLongButtonRect);
                _currentLongButtonRect.BeginAnimation(OpacityProperty, anim);

                _currentLongButton = null;
                _currentLongButtonRect = null;
            }
        }

        private void UpdateLivesDisplay()
        {
            StatusText.Text = $"Жизни: {3 - _gameLogic.MissedButtons}";
        }

        private void OnGameEnded()
        {
            Dispatcher.Invoke(() =>
            {
                _gameTimer.Stop();
                _spawnTimer.Stop();

                string message = $"Игра окончена!\nВаш счет: {_gameLogic.Score}";
                if (_gameLogic.MissedButtons >= 3)
                {
                    message += "\nПричина: Вы потратили все жизни";
                }
                else if (_gameLogic.Score < 0)
                {
                    message += "\nПричина: нажата кнопка-ловушка";
                }

                MessageBox.Show(message, "Результат игры");
                Close();
            });
        }

        private void UpdateScore(int score)
        {
            Dispatcher.Invoke(() =>
            {
                ScoreText.Text = $"Очки: {score}";
                StatusText.Text = $"Жизни: {3 - _gameLogic.MissedButtons}";
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_isClosed) return;
            _isClosed = true;

            _gameTimer.Stop();
            _spawnTimer.Stop();

            _gameLogic.ScoreUpdated -= UpdateScore;
            _gameLogic.GameEnded -= OnGameEnded;
            this.PreviewMouseDown -= HandleButtonPress;
            this.PreviewMouseUp -= HandleButtonRelease;

            if (!_gameLogic.IsGameOver)
            {
                _gameLogic.EndGame();
            }

            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()?.Show();
        }
    }
}