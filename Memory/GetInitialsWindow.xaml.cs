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
    /// Gets the initials of a player that just made a new high score
    /// </summary>
    public partial class GetInitialsWindow : Window
    {
        /// <summary>
        /// references the text box with the initials in it
        /// </summary>
        public string Initials
        {
            get
            {
                return initialsTextBox.Text;
            }
            set
            {
                initialsTextBox.Text = value;
            }
        }

        /// <summary>
        /// The Label that tells the user how they did
        /// </summary>
        public string Message
        {
            get { return (string)roundsLabel.Content; }
            set { roundsLabel.Content = value; }
        }
        public GetInitialsWindow()
        {
            InitializeComponent();
            //set focus to the text box
            initialsTextBox.Focus();
        }

        public void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            if (initialsTextBox.Text == "")
                initialsTextBox.Text = "AAA";
            //todo set DialogResult to false and cancel?
        }
    }
}
