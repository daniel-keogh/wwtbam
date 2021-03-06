using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Data;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TMP_InputField nameInput;

    [Header("Confirmation")]
    [SerializeField] private TextMeshProUGUI saveConfirmationText;
    [Tooltip("The amount of time the confirmation text will be visible")]
    [SerializeField] private float confirmTimeout = 3f;

    private int winnings;
    private bool isSaved;

    private Coroutine confirmCoroutine;
    private GameController gc;

    public string NameInput
    {
        get => nameInput.text;
        set => nameInput.text = value;
    }

    void Start()
    {
        gc = FindObjectOfType<GameController>();

        if (gc != null)
        {
            winnings = gc.GetFinalWinnings();
        }

        scoreText.text += $"{string.Format("{0:n0}", winnings)}";
    }

    /// <summary>
    /// Saves the player's score to the leaderboard.
    /// </summary>
    public void OnSaveScore()
    {
        if (confirmCoroutine != null)
        {
            StopCoroutine(confirmCoroutine);
        }

        if (string.IsNullOrEmpty(nameInput.text.Trim()))
        {
            confirmCoroutine = StartCoroutine(ShowConfirmation("Name can't be empty!"));
            return;
        }

        if (isSaved)
        {
            confirmCoroutine = StartCoroutine(ShowConfirmation("Already saved!"));
            return;
        }

        SaveSystem.SaveToLeaderBoard(new PlayerData
        {
            name = nameInput.text,
            winnings = winnings
        });

        isSaved = true;
        confirmCoroutine = StartCoroutine(ShowConfirmation("Saved!"));
    }

    private IEnumerator ShowConfirmation(string message)
    {
        saveConfirmationText.text = message;
        saveConfirmationText.gameObject.SetActive(true);

        yield return new WaitForSeconds(confirmTimeout);

        saveConfirmationText.gameObject.SetActive(false);
    }
}
