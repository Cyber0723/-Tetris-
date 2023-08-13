using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
        };


        private readonly Image[,] imageControls;
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;

        private GameStates gameStates = new GameStates();

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameStates.GameGrid);
            string konum = System.IO.Directory.GetCurrentDirectory();
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(konum + @"\Assets\Müzik.wav");
            player.Play();
        }

        private  Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }

            return imageControls;
        }

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r,c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Positions p in block.TilePositions())
            {
                imageControls[p.Row,p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(BlockAtaması blockAtaması)
        {
            Block next = blockAtaması.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.Id];
            }
        }


        private void Draw(GameStates gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockAtaması);
            DrawHeldBlock(gameState.HeldBlock);
            ScoreText.Text=$"Puan:{gameState.Score}";
        }

        private async Task GameLoop()
        {
            Draw(gameStates);

            while (!gameStates.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (gameStates.Score * delayDecrease));
                await Task.Delay(delay);
                gameStates.MoveBlockDown();
                Draw(gameStates);
            }
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text= $"Puan:{gameStates.Score}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameStates.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    gameStates.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameStates.MoveBlockRight();
                    break;
                case Key.Down:
                    gameStates.MoveBlockDown();
                    break;
                case Key.Up:
                    gameStates.RotateBlocCW();
                    break;
                case Key.Z:
                    gameStates.RotateBlockCCW();                    
                    break;
                case Key.T:
                    gameStates.HoldBlock();
                    break;
                case Key.Space:
                    gameStates.DropBlock();
                    break;
                default:
                    return;
            }

            Draw(gameStates);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();   
        }
        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameStates = new GameStates();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }
    }
}
