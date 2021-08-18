using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Memory
{
    /// <summary>
    /// The difficulties that the game can be played on
    /// </summary>
    public enum Difficulty
    {
        Easy, Medium, Hard
    }

    /// <summary>
    /// The type of cards that can be used to play the game
    /// </summary>
    public enum CardResources
    {
        Flags, Companies
    }

    [Serializable]
    public class UserPreferences
    {
        #region SERIALIZABLE DATA
        //this section is the instantiable UserPreferences information used to save the user's settings

        public Difficulty selectedDifficulty;
        public CardResources selectedResources;
        public List<HighScore> easyScores, mediumScores, hardScores;

        public UserPreferences()
        {
            selectedDifficulty = UserPreferences.difficulty;
            selectedResources = UserPreferences.resources;

            easyScores = UserPreferences.EasyHighScores;
            mediumScores = UserPreferences.MediumHighScores;
            hardScores = UserPreferences.HardHighScores;
        }

        #endregion

        //Difficulty setting the user has selected (controls number of rows and columns in the grid of cards)
        public static Difficulty difficulty = Difficulty.Easy; //default is easy, changes when UserPreferences is deserialized

        //resources images for the cards
        public static string[] companies = { "Resources/amazon.jpg",
                                            "Resources/android.png",
                                            "Resources/apple.jpg",
                                            "Resources/facebook.png",
                                            "Resources/google.jpg",
                                            "Resources/lg.png",
                                            "Resources/microsoft.jpg",
                                            "Resources/oracle.png",
                                            "Resources/samsung.png",
                                            "Resources/snapchat.jpg",
                                            "Resources/sony.jpg",
                                            "Resources/sprint.png",
                                            "Resources/valve.jpg",
                                            "Resources/verison.png",
                                            "Resources/xbox.png" };

        public static string[] flags = { "Resources/austrailia.png",
                                        "Resources/brazil.png",
                                        "Resources/canada.png",
                                        "Resources/china.png",
                                        "Resources/france.png",
                                        "Resources/germany.png",
                                        "Resources/india.png",
                                        "Resources/japan.png",
                                        "Resources/mexico.png",
                                        "Resources/norway.png",
                                        "Resources/russia.png",
                                        "Resources/southafrica.png",
                                        "Resources/spain.png",
                                        "Resources/unitedkingdom.png",
                                        "Resources/unitedstates.png" };

        //default resources
        public static CardResources resources = CardResources.Flags;
        public static string[] cardResources = flags;

        /// <summary>
        /// Changes the card image resources to the provided resource
        /// </summary>
        /// <param name="r">The selected Resource collection to change to</param>
        public static void ChangeResources(CardResources r)
        {
            switch (r)
            {
                case CardResources.Companies:
                    UserPreferences.cardResources = companies;
                    resources = CardResources.Companies;
                    break;
                default:
                    UserPreferences.cardResources = flags;
                    resources = CardResources.Flags;
                    break;
            }
        }

        //High Scores information
        public static List<HighScore> EasyHighScores = new List<HighScore>();
        public static List<HighScore> MediumHighScores = new List<HighScore>();
        public static List<HighScore> HardHighScores = new List<HighScore>();


        /// <summary>
        /// Saves the information about the game and settings to a file
        /// </summary>
        public static void SerializeUserPreferences()
        {
            using (Stream fileStream = File.Create("UserPreferences.dat"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, new UserPreferences());
            }
        }

        /// <summary>
        /// Attempts to read the saved UserPreferences file from a previous run of the program. If none is found
        /// the default values are used.
        /// </summary>
        public static void DeserializeUserPreferences()
        {
            UserPreferences preferences;

            try
            {
                using (Stream filestream = File.Open("UserPreferences.dat", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    preferences = (UserPreferences)formatter.Deserialize(filestream);
                }
            } catch (Exception e)
            {
                //Use default UserPreferences
                return;
            }

            //the read went okay, set all of the preferences
            EasyHighScores = preferences.easyScores;
            MediumHighScores = preferences.mediumScores;
            HardHighScores = preferences.hardScores;

            difficulty = preferences.selectedDifficulty;
            ChangeResources(preferences.selectedResources);
        }

        #region HIGH SCORES

        /// <summary>
        /// Checks if the score achieved is a new high score
        /// </summary>
        /// <param name="difficulty">The difficulty the user played on</param>
        /// <param name="rounds">The number of turns it took them to complete the game</param>
        /// <returns>True if a new high score</returns>
        public static bool CheckHighScores(Difficulty difficulty, int rounds)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    //if there isn't 3 saved scores yet, then this is a new high score
                    if (EasyHighScores.Count < 3)
                        return true;
                    //else compare the score achieved with the other saved scores
                    foreach (HighScore h in EasyHighScores)
                        if (rounds < h.Score)
                            return true;
                    break;
                    //other difficulties follow similar logic
                case Difficulty.Medium:
                    if (MediumHighScores.Count < 3)
                        return true;
                    foreach (HighScore h in MediumHighScores)
                        if (rounds < h.Score)
                            return true;
                    break;
                case Difficulty.Hard:
                    if (HardHighScores.Count < 3)
                        return true;
                    foreach (HighScore h in HardHighScores)
                        if (rounds < h.Score)
                            return true;
                    break;
            }
            //if we didn't return true before, then we don't have a new score
            return false;
        }

        /// <summary>
        /// Adds a new high score to the list of high scores for the specified difficulty
        /// </summary>
        /// <param name="difficulty">The difficulty to add to</param>
        /// <param name="rounds">The score the user achieved</param>
        /// <param name="initials">The initials of the user</param>
        public static void AddHighScore(Difficulty difficulty, int rounds, string initials)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    //add the new high score
                    EasyHighScores.Add(new HighScore(initials, rounds));
                    //sort the list
                    EasyHighScores.Sort();
                    //truncate so there are only 3
                    if (EasyHighScores.Count > 3)
                        EasyHighScores.RemoveAt(3);
                    break;
                case Difficulty.Medium:
                    MediumHighScores.Add(new HighScore(initials, rounds));
                    MediumHighScores.Sort();
                    if (MediumHighScores.Count > 3)
                        MediumHighScores.RemoveAt(3);
                    break;
                case Difficulty.Hard:
                    HardHighScores.Add(new HighScore(initials, rounds));
                    HardHighScores.Sort();
                    if (HardHighScores.Count > 3)
                        HardHighScores.RemoveAt(3);
                    break;
            }
        }

        #endregion
    }
}
