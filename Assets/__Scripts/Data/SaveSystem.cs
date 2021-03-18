using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Data
{
    public static class SaveSystem
    {
        /// <summary>
        /// Path to the LeaderBoard data file
        /// </summary>
        private static readonly string LEADERBOARD_DATA_PATH = Application.persistentDataPath + "/leaderboard.json";

        /// <summary>
        /// Saves the given PlayerData object to the leaderboard.json file.
        /// </summary>
        public static void SaveToLeaderBoard(PlayerData player)
        {
            // Read the current LeaderBoard
            LeaderBoard current = LoadLeaderBoard();
            List<PlayerData> temp = new List<PlayerData>();

            if (current.players != null)
            {
                temp.AddRange(current.players);
            }

            temp.Add(player);

            // Sort the List in descending order
            temp.Sort();
            temp.Reverse();

            var lb = new LeaderBoard
            {
                players = temp
            };

            File.WriteAllText(LEADERBOARD_DATA_PATH, lb.ToString());
        }

        /// <summary>
        /// Loads the existing leaderboard from the filesystem, or creates a new one if the
        /// file doesn't exist.
        /// </summary>
        public static LeaderBoard LoadLeaderBoard()
        {
            try
            {
                string json = File.ReadAllText(LEADERBOARD_DATA_PATH);
                return JsonUtility.FromJson<LeaderBoard>(json);
            }
            catch (FileNotFoundException)
            {
                return new LeaderBoard();
            }
        }

        /// <summary>
        /// Deletes the leaderboard.json file.
        /// </summary>
        public static void ClearLeaderBoard()
        {
            if (File.Exists(LEADERBOARD_DATA_PATH))
            {
                File.Delete(LEADERBOARD_DATA_PATH);
            }
        }
    }
}
