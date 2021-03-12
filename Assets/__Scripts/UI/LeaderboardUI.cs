using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Data;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardItemPrefab;
    [SerializeField] private VerticalLayoutGroup layoutGroup;

    void Start()
    {
        PrintItems();
    }

    private void PrintItems()
    {
        // Read the top scores from the leaderboard file and print them to the screen
        var leaderBoard = SaveSystem.LoadLeaderBoard();

        if (leaderBoard.players != null)
        {
            for (int i = 0; i < leaderBoard.players.Count; i++)
            {
                var item = Instantiate(
                    leaderboardItemPrefab,
                    layoutGroup.transform,
                    false
                );

                SetPlayerData(item, leaderBoard.players[i], i + 1);
            }
        }
    }

    private void SetPlayerData(GameObject item, PlayerData player, int rank)
    {
        // Format the Leaderboard item so it looks like:
        //
        // Name                                 Winnings
        //
        // https://forum.unity.com/threads/textmeshpro-right-and-left-align-on-same-line.485157/
        //
        item.GetComponentInChildren<TextMeshProUGUI>().text = (
            $@"<align=left>{rank}. {player.name}<line-height=0.001>
            <align=right>€{string.Format("{0:n0}", player.winnings)}"
        );
    }
}
