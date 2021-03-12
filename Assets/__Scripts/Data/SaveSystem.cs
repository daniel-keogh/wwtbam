using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Data
{
    public static class SaveSystem
    {
        // Path to the LeaderBoard data file
        private static readonly string LEADERBOARD_DATA_PATH = Application.persistentDataPath + "/leaderboard.json";

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

            // Save the new LeaderBoard to the leaderboard.json file
            var lb = new LeaderBoard
            {
                players = temp
            };

            File.WriteAllText(LEADERBOARD_DATA_PATH, lb.ToString());
        }

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

        public static void ClearLeaderBoard()
        {
            if (File.Exists(LEADERBOARD_DATA_PATH))
            {
                File.Delete(LEADERBOARD_DATA_PATH);
            }
        }
    }
}
