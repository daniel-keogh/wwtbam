using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class PrizeItem : MonoBehaviour
{
    [Header("Colours")]
    [SerializeField] private Color isActiveBackgroundColor;
    [SerializeField] private Color isSafetyTextColor;
    [SerializeField] private Color isActiveTextColor;

    [Header("Status")]
    [SerializeField] private bool isActive;
    [SerializeField] private bool isSafetyNet;

    private TextMeshProUGUI childText;
    private Image backgroundImage;

    private Color inActiveTextColor;
    private Color inActiveBackgroundColor;
    private GameController gc;
    private int questionNumber;
    private int prizeValue;

    public int QuestionNumber
    {
        get => questionNumber;
        set => questionNumber = value;
    }

    public int PrizeValue
    {
        get => prizeValue;
        set => prizeValue = value;
    }

    void Start()
    {
        gc = FindObjectOfType<GameController>();
        childText = GetComponentInChildren<TextMeshProUGUI>();
        backgroundImage = GetComponent<Image>();

        inActiveTextColor = childText.color;
        inActiveBackgroundColor = backgroundImage.color;
    }

    void Update()
    {
        // Update the item depending on whether it is currently active or not
        isActive = gc.CurrentWinnings == prizeValue;
        isSafetyNet = gc.SafetyNets.Contains(prizeValue);

        if (isActive)
        {
            backgroundImage.color = isActiveBackgroundColor;
            childText.color = isActiveTextColor;
        }
        else
        {
            backgroundImage.color = inActiveBackgroundColor;
            childText.color = inActiveTextColor;
        }

        if (isSafetyNet)
        {
            childText.color = isSafetyTextColor;
        }

        childText.text = $"{questionNumber}.\t€{string.Format("{0:n0}", prizeValue)}";
    }
}
