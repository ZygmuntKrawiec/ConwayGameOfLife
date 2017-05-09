using System.Windows;
using ConwayGameEngineLibrary;

namespace ConwayGameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //A game board displays on the main window.
        ConwayGameBoard cgBoard = new ConwayGameBoard();
        //An engine of the game for a board with 100x100 dimensions
        ConwayGameEngine cgEngine = new ConwayGameEngine(100, 100, 10);

        public MainWindow()
        {
            InitializeComponent();

            //Connects the engine with the display.
            cgEngine.LifeCycleDone += cgBoard.SetValuesToArrayCells;

            //Some tricks to make a life easier. After aleft mouse button is down it changes state of lifes in the game engine.
            cgBoard.RectangleMouseLeftButtonDown += (x, i, j) => { cgEngine.BaseGameBoard[i, j] = x; };

            //Runs the game.
            cgEngine.Start();

            //Adds the board to the main window.
            gridConwayGame.Children.Add(cgBoard.Board);
        }

        /// <summary>
        /// Stops or runs Conway Game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Pause_Click(object sender, RoutedEventArgs e)
        {
            if (cgEngine.IsEngineRunning)
            {
                cgEngine.Stop();
                btn_Pause.Content = "Run";
            }
            else
            {
                cgEngine.Start();
                btn_Pause.Content = "Pause";
            }
        }

        /// <summary>
        /// Clears a whole game board.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            cgEngine.Reset();
        }
    }
}
