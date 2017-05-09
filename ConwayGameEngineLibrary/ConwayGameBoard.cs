using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ConwayGameEngineLibrary
{
    public class ConwayGameBoard
    {
        //Fields
        /// <summary>
        /// A main board of the game. 
        /// </summary>
        Canvas board = new Canvas();
        /// <summary>
        /// An array of rectangles used to display a state of lifes, depending on the colors.
        /// example: A white rectangle represents a life, a black rectangle represents empty place.
        /// </summary>
        Rectangle[,] arrayOfLifes;

        //Properties
        public Rectangle[,] ArrayOfLifes
        {
            get
            {
                return arrayOfLifes;
            }

            set
            {
                arrayOfLifes = value;
            }
        }
        public Canvas Board
        {
            get { return board; }
        }

        //Event
        /// <summary>
        ///  Occurs when the left mouse button is pressed while the mouse pointer is over a rectangle in board game.
        /// </summary>
        public event Action<bool,int,int> RectangleMouseLeftButtonDown;

        //Private methods
        /// <summary>
        /// Initializes the ArrayOfLifes.
        /// </summary>
        /// <param name="rows">Rows of initialized array.</param>
        /// <param name="columns">Columns off initializes array.</param>
        private void initialiseArrayOfLifes(int rows, int columns)
        {
            arrayOfLifes = new Rectangle[rows, columns];
            //Initialises all rectangles and adds MousButtonDown event handler to all rectangles.
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    ArrayOfLifes[i, j] = new Rectangle() { Margin = new Thickness(i * 4, j * 4, 0, 0), Width = 4, Height = 4, Fill = Brushes.White };
                    ArrayOfLifes[i, j].MouseLeftButtonDown += ConwayGameBoard_MouseLeftButtonDown;
                }
            }
            addRectanglesToBoard();
            //Sets board's height and width according to array's dimensions.
            Board.Height = rows * 4;
            Board.Width = columns * 4;
        }

        /// <summary>
        /// Occurs after a left button is pressed over a rectangle on the board.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConwayGameBoard_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (((Rectangle)sender).Fill == Brushes.Black)
            {
                //If the rectangle has a black background then it changes to white and invoke event RectangleMouseLeftButtonDown  
                ((Rectangle)sender).Fill = Brushes.White;
                RectangleMouseLeftButtonDown?.Invoke(true, (int)(((Rectangle)sender).Margin.Left / 4), (int)((Rectangle)sender).Margin.Top / 4);
                
            }
            else
            {
                //If the rectangle has a white background then it changes to black and invoke event RectangleMouseLeftButtonDown 
                ((Rectangle)sender).Fill = Brushes.Black;
                RectangleMouseLeftButtonDown?.Invoke(false, (int)(((Rectangle)sender).Margin.Left / 4), (int)((Rectangle)sender).Margin.Top / 4);
            }
        }

        /// <summary>
        /// Adds an array of rectangles to the game board.
        /// </summary>
        private void addRectanglesToBoard()
        {
            foreach (Rectangle rect in arrayOfLifes)
            {
                board.Children.Add(rect);
            }
        }

        //Public methods
        /// <summary>
        /// Sets proper color to cells in array of lifes
        /// </summary>
        /// <param name="arrayOfValues">Array points to which cells should changed the color. "True" value sets white and 'false" sets black color.</param>
        public void SetValuesToArrayCells(bool[,] arrayOfValues)
        {
            //If the arrayOfLifes does not exist or arrayOFValues has changed dimension 
            //then create a new arrayOfLifes and initialize with proper values.
            if (arrayOfValues == null)
            {
                throw new Exception("Array cannot be null");
            }
            else if (arrayOfLifes == null ||
                     arrayOfLifes.GetLength(0) != arrayOfValues.GetLength(0) ||
                     arrayOfLifes.GetLength(1) != arrayOfValues.GetLength(1))
            {
                initialiseArrayOfLifes(arrayOfValues.GetLength(0), arrayOfValues.GetLength(1));
            }
            //Sets proper colors to cells in the arrayOfLifes.
            for (int i = 0; i < arrayOfValues.GetLength(0); i++)
            {
                for (int j = 0; j < arrayOfValues.GetLength(1); j++)
                {
                    arrayOfLifes[i, j].Fill = arrayOfValues[i, j] == true ? Brushes.White : Brushes.Black;
                }
            }

        }
    }
}
