using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrizeDisplay : MonoBehaviour
{
    [SerializeField] private PrizeItem prizeItemPrefab;
    [SerializeField] private LayoutGroup layoutGroup;

    void Start()
    {
        ListPrizes();
    }

    private void ListPrizes()
    {
        // Print out the list of scores/prizes
        var gc = FindObjectOfType<GameController>();

        for (int i = gc.Prizes.Count - 1; i >= 0; i--)
        {
            var item = Instantiate<PrizeItem>(
                prizeItemPrefab,
                layoutGroup.transform,
                false
            );
            item.QuestionNumber = i + 1;
            item.PrizeValue = gc.Prizes[i];
        }
    }
}
