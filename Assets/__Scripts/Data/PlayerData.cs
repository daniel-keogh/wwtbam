using System;
using System.Collections;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class PlayerData : Model, IComparable<PlayerData>
    {
        public string name;
        public int winnings;

        public int CompareTo(PlayerData other)
        {
            // Sort by the value of `winnings`
            return this.winnings.CompareTo(other.winnings);
        }
    }

    [Serializable]
    public class LeaderBoard : Model
    {
        public List<PlayerData> players;
    }
}
