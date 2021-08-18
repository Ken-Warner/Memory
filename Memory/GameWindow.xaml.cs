using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Memory
{
    /// <summary>
    /// The window where the Memory game takes place
    /// </summary>
    public partial class GameWindow : Window
    {
        //the difficulty of the game
        public Difficulty difficulty { get; set; }
        //references the CardResources the player will be using for this game
        public string[] cardResources { get; set; }

        //keeps track of the number of attempted matches from the player
        public int NumberOfRounds { get; private set; }
        //references to the cards the user has interacted with
        private Card firstCard, secondCard;

        //keeps track of the game state
        private bool isFirstCardFlipped;
        //Used by the timer, if false, the user can't interact with the UI
        private bool canFlip;
        //timer is used to delay the UI so the user can see which cards were flipped (and locks the UI with canFlip)
        private DispatcherTimer timer;

        //depending on difficulty, different numbers of cards need to fit in the UI, these values change accordingly
        private int gridRows, gridCols, cardWidth, cardHeight, pairsRemaining;
        //the list of cards to be displayed on the grid (easier difficulties don't use all of the resources)
        private List<Card> cards;

        public GameWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set the initial properties of the game
        /// </summary>
        /// <param name="difficulty">The difficulty the game is on</param>
        public void InitializeGameComponents()
        {
            //some initial properties
            NumberOfRounds = 0;
            isFirstCardFlipped = false;
            canFlip = true;
            cards = new List<Card>();

            //initialize the timer used within the game
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += TimerTick;

            //establish game board dimensions based upon the difficulty
            switch (difficulty)
            {
                case Difficulty.Easy:
                    gridRows = 2;
                    gridCols = 5;
                    cardWidth = 125;
                    cardHeight = 65;
                    Height = 200; //changes the height of the game window if there are this few of cards
                    break;
                case Difficulty.Medium:
                    gridRows = 4;
                    gridCols = 5;
                    cardWidth = 125;
                    cardHeight = 65;
                    break;
                case Difficulty.Hard:
                    gridRows = 5;
                    gridCols = 6;
                    cardWidth = 125;
                    cardHeight = 65;
                    break;
                default:
                    gridRows = 2;
                    gridCols = 5;
                    cardWidth = 125;
                    cardHeight = 65;
                    break;
            }

            //set pairs remaining
            pairsRemaining = (gridRows * gridCols) / 2;
            InitializeGameGrid();
            PopulateGameGrid();
        }

        /// <summary>
        /// Create the grid in the window to house the card elements
        /// </summary>
        private void InitializeGameGrid()
        {
            gameGrid.ShowGridLines = false;

            //start with the columns
            ColumnDefinition column = new ColumnDefinition();
            column.Width = new GridLength(1, GridUnitType.Star); //padding column
            gameGrid.ColumnDefinitions.Add(column);

            //for each of the columns we create a definition
            for (int i = 0; i < gridCols; i++)
            {
                column = new ColumnDefinition();
                column.Width = GridLength.Auto;
                gameGrid.ColumnDefinitions.Add(column);
            }

            //use one more column to fill the remaining space in the window
            column = new ColumnDefinition();
            column.Width = new GridLength(1, GridUnitType.Star);
            gameGrid.ColumnDefinitions.Add(column);

            //The rows of the grid follow a similar logic
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(1, GridUnitType.Star);
            gameGrid.RowDefinitions.Add(row);
            for (int i = 0; i < gridRows; i++)
            {
                row = new RowDefinition();
                row.Height = GridLength.Auto;
                gameGrid.RowDefinitions.Add(row);
            }
            row = new RowDefinition();
            row.Height = new GridLength(1, GridUnitType.Star);
            gameGrid.RowDefinitions.Add(row);
        }

        /// <summary>
        /// Populates the game grid with Cards
        /// </summary>
        private void PopulateGameGrid()
        {
            //references to the current pair of cards
            Card card1, card2;
            //reference to the image for these cards
            BitmapImage image;

            for (int imageIterator = 0; imageIterator < pairsRemaining; imageIterator++)
            {
                //get the bitmap for the front of the card
                image = new BitmapImage(new Uri(cardResources[imageIterator], UriKind.Relative));

                //instantiate two distinct cards with the same front image
                card1 = new Card(image, cardWidth, cardHeight, imageIterator);
                card2 = new Card(image, cardWidth, cardHeight, imageIterator);

                //let the game window handle their click events
                card1.MouseUp += CardClicked;
                card2.MouseUp += CardClicked;

                //throw them in the deck for this game
                cards.Add(card1);
                cards.Add(card2);

                //move to the next image;
            }

            //shuffle the deck
            Random shuffler = new Random();
            for (int i = cards.Count - 1; i > 0; i--)
            {
                Card c = cards[i];
                int indexToSwithWith = shuffler.Next(i);
                cards[i] = cards[indexToSwithWith];
                cards[indexToSwithWith] = c;
            }

            //place the cards in the grid
            int currentCard = 0;
            for (int row = 1; row <= gridRows; row++)
                for (int col = 1; col <= gridCols; col++)
                {
                    Grid.SetColumn(cards[currentCard], col);
                    Grid.SetRow(cards[currentCard], row);
                    gameGrid.Children.Add(cards[currentCard]);
                    currentCard += 1;
                }
        }

        /// <summary>
        /// Temporarily locks the UI giving the User time to view their selections
        /// </summary>
        public void TimerTick(object sender, EventArgs e)
        {
            //when the timer ticks, it means the second card has been pressed and 1 second has passed

            //this method must determine if the two cards are to be flipped
            //back over or if they match and therefore remain face up

            //increment the number of rounds
            NumberOfRounds += 1;

            if (!(secondCard.ID == firstCard.ID)) //no match
            {
                firstCard.FlipToReverse();
                secondCard.FlipToReverse();
            } else //match
            {
                //one less pair remaining
                pairsRemaining -= 1;
                //check for game end state
                if (pairsRemaining == 0)
                {
                    DialogResult = true;
                    Close();
                }
            }

            //if the game isn't over yet, change the state to no cards flipped
            isFirstCardFlipped = false;

            //let the user interact with the UI again
            canFlip = true;

            //stop the timer
            timer.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CardClicked(object sender, RoutedEventArgs e)
        {
            //if the UI is locked, the user can do anything to it
            if (!canFlip)
                return;

            //get the reference to the card
            Card thisCard = (Card)sender;

            //attempt to flip it
            bool flipped = thisCard.Flip();

            //if it couldn't be flipped
            if (!flipped)
            {
                MessageBox.Show("This card is already face up!");
            } else
            { //if it could
                //is this the first or second card in the attempted pair
                if (isFirstCardFlipped) //second
                {
                    //set the timer and lock the UI
                    secondCard = thisCard;
                    timer.Start();
                    canFlip = false;
                } else //first
                {
                    firstCard = thisCard;
                    isFirstCardFlipped = true;
                }
            }
            e.Handled = true;
        }
    }
}
