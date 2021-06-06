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
        }

        /// <summary>
        /// Triggers when the start button is clicked. Loads a new game window and starts the game.
        /// </summary>
        public void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //todo Implement
        }

        /// <summary>
        /// Triggers when the settings button is clicked. Loads a new settings window.
        /// </summary>
        public void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            //todo Implement
        }

        /// <summary>
        /// Ends the game and closes the window.
        /// </summary>
        public void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            e.Handled = true;
        }

        /// <summary>
        /// Serializes the UserPreferences class that will house the settings and high scores before closing the window and
        /// ending the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //todo Serialize User Preferences class to save play mode/resources/difficulty/high scores/etc
        }
    }
}
