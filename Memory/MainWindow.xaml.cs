using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Memory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //load user settings
            UserPreferences.DeserializeUserPreferences();
        }

        /// <summary>
        /// Triggers when the start button is clicked. Loads a new game window and starts the game.
        /// </summary>
        public void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //instantiate and initialize the game window
            GameWindow gWindow = new GameWindow()
            {
                difficulty = UserPreferences.difficulty,
                cardResources = UserPreferences.cardResources
            };

            gWindow.InitializeGameComponents();


            //hide this window and shoe the game window
            Hide();
            bool? result = gWindow.ShowDialog();
            Show();

            //once the game is done, if it closed appropriately, check for a high score
            if (result.HasValue)
            {
                if (result.Value)
                {
                    bool isHighScore = UserPreferences.CheckHighScores(UserPreferences.difficulty, gWindow.NumberOfRounds);
                    if (isHighScore)
                    {
                        //get the initials of the player if we have a high score
                        GetInitialsWindow initialsWindow = new GetInitialsWindow();
                        initialsWindow.Message = "Congratulations, " + gWindow.NumberOfRounds + Environment.NewLine + " is a new high score!";
                        bool? initialsResult = initialsWindow.ShowDialog();

                        string initials;
                        if (initialsResult.HasValue && initialsResult.Value)
                            initials = initialsWindow.Initials;
                        else
                            initials = "AAA";

                        //save the high score
                        UserPreferences.AddHighScore(UserPreferences.difficulty, gWindow.NumberOfRounds, initials);
                    } else
                    {
                        //no high score
                        MessageBox.Show("You completed the game in " + gWindow.NumberOfRounds + " rounds!", "Congratulations!");
                    }
                }
            }
            //if no value then the game window exited prematurely (x button)
        }

        /// <summary>
        /// Triggers when the settings button is clicked. Loads a new settings window.
        /// </summary>
        public void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            //create the window
            SettingsWindow settingsWindow = new SettingsWindow()
            {
                SelectedDifficulty = UserPreferences.difficulty,
                SelectedResources = UserPreferences.resources
            };

            //hide this window and show the settings window, reshow when done
            Hide();
            bool? result = settingsWindow.ShowDialog();
            Show();

            //if window was closed legitimately and it was the save button that closed it
            if (result.HasValue && result.Value)
            {
                //update UserPreferences
                UserPreferences.difficulty = settingsWindow.SelectedDifficulty;
                UserPreferences.ChangeResources(settingsWindow.SelectedResources);
            }

            e.Handled = true; //mark event as handled
        }

        /// <summary>
        /// Ends the game and closes the window.
        /// </summary>
        public void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Close();
        }

        /// <summary>
        /// Serializes the UserPreferences class that will house the settings and high scores before closing the window and
        /// ending the game
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //save settings
            UserPreferences.SerializeUserPreferences();
        }
    }
}
