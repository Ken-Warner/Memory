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

namespace Memory
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public Difficulty SelectedDifficulty
        {
            get
            {
                //return difficulty based uppon the state of the form
                if ((bool)easyRadioButton.IsChecked)
                    return Difficulty.Easy;
                else if ((bool)mediumRadioButton.IsChecked)
                    return Difficulty.Medium;
                else if ((bool)hardRadioButton.IsChecked)
                    return Difficulty.Hard;
                else
                    return Difficulty.Easy; //default if something went wrong
            }
            set
            {
                //set the form elements based upon the given difficulty
                if (value == Difficulty.Hard)
                {
                    easyRadioButton.IsChecked = false;
                    mediumRadioButton.IsChecked = false;
                    hardRadioButton.IsChecked = true;
                } else if (value == Difficulty.Medium)
                {
                    easyRadioButton.IsChecked = false;
                    mediumRadioButton.IsChecked = true;
                    hardRadioButton.IsChecked = false;
                } else //use easy as default
                {
                    easyRadioButton.IsChecked = true;
                    mediumRadioButton.IsChecked = false;
                    hardRadioButton.IsChecked = false;
                }
            }
        }


        public CardResources SelectedResources
        {
            //getters and setters follow similar logic to 'SelectedDifficulty'
            get
            {
                if ((bool)companyRadioButton.IsChecked)
                    return CardResources.Companies;
                else
                    return CardResources.Flags;
            }
            set
            {
                if (value == CardResources.Companies)
                {
                    companyRadioButton.IsChecked = true;
                    flagRadioButton.IsChecked = false;
                } else
                {
                    companyRadioButton.IsChecked = false;
                    flagRadioButton.IsChecked = true;
                }
            }
        }


        public SettingsWindow()
        {
            InitializeComponent();

            //put all of the information for high scores into place
            SetUpHighScoresGrid();
        }

        /// <summary>
        /// Takes the UserPreferences HighScores and places them systematically into the grid.
        /// </summary>
        private void SetUpHighScoresGrid()
        {
            //textblock reference
            TextBlock textBlock;
            //loop through each row in each column of the grid
            for (int col = 1; col < 4; col++)
                for (int row = 2; row < 5; row++)
                {
                    //get the high score for the appropriate cell
                    HighScore thisHighScore;
                    if (col == 1)
                        thisHighScore = (UserPreferences.EasyHighScores.Count >= row - 1) ? UserPreferences.EasyHighScores[row - 2] : null;
                    else if (col == 2)
                        thisHighScore = (UserPreferences.MediumHighScores.Count >= row - 1) ? UserPreferences.MediumHighScores[row - 2] : null;
                    else
                        thisHighScore = (UserPreferences.HardHighScores.Count >= row - 1) ? UserPreferences.HardHighScores[row - 2] : null;

                    //check if there is a score in this cell yet
                    int score;
                    string initials;
                    if (thisHighScore != null)
                    {
                        score = thisHighScore.Score;
                        initials = thisHighScore.Initials;
                    }
                    else
                    {   //if not, just set some blank data
                        score = 0;
                        initials = "N/A";
                    }

                    //create a text box with it
                    textBlock = new TextBlock
                    {
                        Text = score + " by " + initials,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };

                    //add it to the grid
                    Grid.SetRow(textBlock, row);
                    Grid.SetColumn(textBlock, col);
                    highScoresGrid.Children.Add(textBlock);
                }
        }

        /// <summary>
        /// Requests that this information be saved and closes the form
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            e.Handled = true;
            Close();
        }

        /// <summary>
        /// Requests that any changes be discarded and closes the form
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            e.Handled = true;
            Close();
        }
    }
}
