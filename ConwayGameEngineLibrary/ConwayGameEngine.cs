using System;
using System.Windows.Threading;

namespace ConwayGameEngineLibrary
{
    public class ConwayGameEngine
    {
        //Fields
        /// <summary>
        /// An array of lifes existence. A "true" value represents existence of life in one cell of an arrary.
        /// </summary>
        bool[,] baseGameBoard;
        /// <summary>
        /// An array to temporary keep calculated new lifes.
        /// </summary>
        bool[,] tempGameBoard;
        /// <summary>
        /// Runs whole game engine mechanism.
        /// </summary>
        DispatcherTimer gameEngineSpark = new DispatcherTimer();

        //Properties
        /// <summary>
        /// Returns/Sets state of lifes on the game board.
        /// </summary>
        public bool[,] BaseGameBoard
        {
            get { return baseGameBoard; }
            set { baseGameBoard = value; }
        }
        /// <summary>
        /// Gets a value that indicates whether the engine is running.
        /// </summary>
        public bool IsEngineRunning
        {
            get { return gameEngineSpark.IsEnabled; }
        }

        //Events
        /// <summary>
        /// Occurs after lifes existence on the game board is recalculated.
        /// </summary>
        public event Action<bool[,]> LifeCycleDone;

        //Constructors
        /// <summary>
        /// Private parameterless constructor helps to initialize common fields in a new instance of ConwayGameEngine. 
        /// </summary>
        private ConwayGameEngine()
        {
            gameEngineSpark.Tick += gameEngineRun;
            gameEngineSpark.Interval =TimeSpan.FromMilliseconds(200);
        }
        /// <summary>
        /// Initializes a new instance of ConwayGameEngine.
        /// </summary>
        /// <param name="width">Width of an array with lifes.</param>
        /// <param name="height">Height of an array of lifes.</param>
        /// <param name="probabilityOfLife">Probability of a life existence in one arrays cell.</param>
        public ConwayGameEngine(int width, int height, int probabilityOfLife = 25) : this()
        {
            baseGameBoard = new bool[height, width];
            tempGameBoard = new bool[height, width];
            //Checks if probability is less than 100%, otherwise sets probability to 25%.
            int probability = probabilityOfLife < 100 ? probabilityOfLife : 25;
            //Initializes an array of lifes existence with a proper probability. 
            initializeBoardGame(probability);
        }
        /// <summary>
        /// Initializes a new instance of ConwayGameEngine.
        /// </summary>
        /// <param name="arrayOfLifes">An array with begining state of lifes. A "true" value of a cell in array indicates a life existence. </param>
        public ConwayGameEngine(bool[,] arrayOfLifes) : this()
        {
            Array.Copy(arrayOfLifes, baseGameBoard, arrayOfLifes.Length);
            tempGameBoard = new bool[arrayOfLifes.GetLength(0), arrayOfLifes.GetLength(1)];
        }

        //Private methods
        /// <summary>
        /// Initializes an array of lifes existence with a proper probability. 
        /// </summary>
        /// <param name="probabilityOfLifeExisting">Probability of life existence. A value range from 0 to 99%.</param>
        private void initializeBoardGame(int probabilityOfLifeExisting)
        {
            Random rand = new Random();
            int live;
            if (baseGameBoard != null)
            {
                for (int i = 0; i < baseGameBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < baseGameBoard.GetLength(1); j++)
                    {
                        live = (int)(rand.Next(0, 100));
                        if (live < probabilityOfLifeExisting)
                        {
                            //It's a live.
                            baseGameBoard[i, j] = true;
                        }
                        else
                        {
                            //It's dead.
                            baseGameBoard[i, j] = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new state of lifes in temporary array, calculated from the state of lifes in baseGameBoard.
        /// Copies a new state of lifes from the temporary array to baseGameBoard.
        /// </summary>
        private void calculateLifeCircle()
        {
            for (int i = 0; i < baseGameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < baseGameBoard.GetLength(1); j++)
                {
                    tempGameBoard[i, j] = calculateLifesAroundPoints(i, j);
                }
            }
            //
            Array.Copy(tempGameBoard, baseGameBoard, tempGameBoard.Length);
        }

        /// <summary>
        /// Calculates lifes around one cell, and depending on the number of lifes around, 
        /// it kills the cell or make it alive.
        /// </summary>
        /// <param name="i">A row number of the cell.</param>
        /// <param name="j">A column number of the cell.</param>
        /// <returns>Returns "true" value if the cell is alive or "false"  if the cell was killed.</returns>
        private bool calculateLifesAroundPoints(int i, int j)
        {
            int x, y, retNumberOfLife = 0;
            bool result = false;
            for (int k = i - 1; k <= i + 1; k++)
            {
                x = checkIfIndexIsOutOfRange(k, baseGameBoard.GetLength(0));
                for (int l = j - 1; l <= j + 1; l++)
                {
                    y = checkIfIndexIsOutOfRange(l, baseGameBoard.GetLength(1));
                    if (checkIfIndexesAreNotBase(k, l, i, j))
                    {
                        if (baseGameBoard[x, y] != false)
                        {
                            retNumberOfLife++;
                        }
                    }
                }
            }
            if (baseGameBoard[i, j] != false) result = (retNumberOfLife >= 2 && retNumberOfLife <= 3) ? true : false;
            else
            if (baseGameBoard[i, j] == false) result = (retNumberOfLife == 3) ? true : false;

            return result;
        }

        /// <summary>
        /// Checks if an index is out of array range, if so then it sets an index on the other side of an array.
        /// </summary>
        /// <param name="i">Examinated index</param>
        /// <param name="maxBoardLength">Max lenght of the row.</param>
        /// <returns></returns>
        private int checkIfIndexIsOutOfRange(int i, int maxBoardLength)
        {
            if (i < 0) return maxBoardLength - 1;
            if (i > maxBoardLength - 1) return 0;
            return i;
        }

        /// <summary>
        /// Checks if a considered index is not a index of investigated cell.
        /// </summary>
        /// <param name="k"></param>
        /// <param name="l"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private bool checkIfIndexesAreNotBase(int k, int l, int i, int j)
        {
            bool returnValue = true;
            if (k == i)
            {
                if (l == j)
                {
                    returnValue = false;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Runs the recalculating proces of the current lifes existence. 
        /// Invokes LifeCycleDone event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gameEngineRun(object sender, EventArgs e)
        {
            calculateLifeCircle();
            LifeCycleDone?.Invoke(baseGameBoard);
        }

        //Public methods
        /// <summary>
        /// Runs Game Engine
        /// </summary>
        /// <param name="miliseconds">Sets how fast a game engine works. 500 miliseconds is a default value.</param>
        public void Start()
        {
            gameEngineSpark.Start();
        }

        /// <summary>
        /// Stops the game engine.
        /// </summary>
        public void Stop()
        {
            gameEngineSpark.Stop();
        }

        ///<summary>
        ///Clears the game.
        ///</summary> 
        public void Reset()
        {
            Array.Clear(BaseGameBoard, 0, BaseGameBoard.Length);
        }
    }

}
