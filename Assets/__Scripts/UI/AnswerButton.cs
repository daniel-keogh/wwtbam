using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Grammars;
using TMPro;

public class AnswerButton : MonoBehaviour
{
    [SerializeField] private Answer answerValue;
    [SerializeField] private AudioClip finalAnswerClip;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI answerText;
    [SerializeField] private TextMeshProUGUI letterText;

    [Header("Backgrounds")]
    [SerializeField] private GameObject selectedBackground;
    [SerializeField] private GameObject answerBackground;

    private bool isSelected = false;
    private bool isDisabled = false;

    private Color normalTextColor;
    private Color normalLetterColor;

    private List<AnswerButton> answerButtons;
    private GameController gameController;
    private SoundController soundController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        soundController = FindObjectOfType<SoundController>();

        normalTextColor = answerText.color;
        normalLetterColor = letterText.color;

        // Maintain a list of the other buttons
        answerButtons = FindObjectsOfType<AnswerButton>()
            .Where(ab => ab != this)
            .ToList();
    }

    void OnEnable()
    {
        GameGrammar.OnAnswerSelected += OnAnswerSelectedEvent;
        GameGrammar.OnFinalAnswer += OnFinalAnswerEvent;
    }

    void OnDisable()
    {
        GameGrammar.OnAnswerSelected -= OnAnswerSelectedEvent;
        GameGrammar.OnFinalAnswer -= OnFinalAnswerEvent;
    }

    public void OnClick()
    {
        if (isSelected && !isDisabled)
        {
            StartCoroutine(CheckAnswer());
        }
        else
        {
            SetSelected(true);
        }
    }

    private void OnAnswerSelectedEvent(Answer answer)
    {
        if (answer == answerValue)
        {
            SetSelected(true);
        }
    }

    private void OnFinalAnswerEvent()
    {
        if (isSelected && !isDisabled)
        {
            StartCoroutine(CheckAnswer());
        }
    }

    private IEnumerator CheckAnswer()
    {
        Answer correctAnswer = gameController.CorrectAnswer;

        // Momentarily prevent the user from pressing other buttons
        DisableAll(true);

        soundController.PlayOneShot(finalAnswerClip);

        yield return new WaitForSeconds(gameController.RevealAnswerDelay);

        // Indicate whether or not the chosen answer was correct
        if (answerValue == correctAnswer)
        {
            SetSelected(false);
            answerBackground.SetActive(true);

            StartCoroutine(gameController.LoadNextQuestion());
        }
        else
        {
            answerButtons.ForEach(ab =>
            {
                if (ab.answerValue == correctAnswer)
                {
                    ab.answerBackground.SetActive(true);
                    ab.letterText.color = Color.black;
                    ab.answerText.color = Color.black;
                }
            });

            StartCoroutine(gameController.EndGame());
        }
    }

    private void SetSelected(bool flag)
    {
        if (!isDisabled)
        {
            isSelected = flag;
            selectedBackground.SetActive(flag);

            if (isSelected)
            {
                answerButtons.ForEach(ab => ab.SetSelected(false));
                gameController.StatusText = "Is that your final answer?";

                letterText.color = Color.black;
                answerText.color = Color.black;
            }
            else
            {
                answerText.color = normalTextColor;
                letterText.color = normalLetterColor;
            }
        }
    }

    /// <summary>
    /// Disables the button assosiated with the provided answer.
    /// </summary>
    public static void DisableAnswer(Answer answer)
    {
        foreach (var button in FindObjectsOfType<AnswerButton>())
        {
            if (button.answerValue == answer)
            {
                button.isDisabled = true;
                button.SetSelected(false);
                button.answerBackground.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Resets all the buttons in the scene to their initial state.
    /// </summary>
    public static void ResetAll()
    {
        foreach (var button in FindObjectsOfType<AnswerButton>())
        {
            button.isDisabled = false;
            button.SetSelected(false);
            button.answerBackground.SetActive(false);
        }
    }

    private static void DisableAll(bool flag)
    {
        foreach (var ab in FindObjectsOfType<AnswerButton>())
        {
            ab.isDisabled = flag;
        }
    }
}
