using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memory
{
    /// <summary>
    /// The card that will be used for the memory game. It is essential an Image but with extra properties for a front and back
    /// and some methods that flip it over.
    /// </summary>
    class Card : Image
    {
        //one bitmap for the front of the card and one for the back of the card
        private BitmapImage backSource;
        private BitmapImage frontSource;

        public bool IsFrontSideUp { get; private set; }

        //this ID matches the other card on the board that is paired with this one
        public int ID { get; private set; }

        public Card(BitmapImage frontSource, int width, int height, int id) : base()
        {
            backSource = new BitmapImage(new Uri("Resources/cardreverse.png", UriKind.Relative));
            this.frontSource = frontSource;

            //set the source for the card to start face down
            Source = backSource;

            //set the image properties for the image element
            Stretch = Stretch.Fill;
            Width = width;
            Height = height;

            //set the id that is unique to every pair of cards
            ID = id;
        }

        /// <summary>
        /// Flips the card over if it is face down.
        /// </summary>
        /// <returns>True if successful flip</returns>
        public bool Flip()
        {
            if (!IsFrontSideUp)
            {
                Source = frontSource;
                IsFrontSideUp = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Flips the card to face down.
        /// </summary>
        public void FlipToReverse()
        {
            Source = backSource;
            IsFrontSideUp = false;
        }
    }
}
