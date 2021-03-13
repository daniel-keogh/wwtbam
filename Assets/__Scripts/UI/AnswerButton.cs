using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Grammars;
using Utilities;

public class AnswerButton : MonoBehaviour
{
    [SerializeField] private Answer answerValue;

    [Header("Audio")]
    [SerializeField] private AudioClip finalAnswerClip;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI letterText;
    [SerializeField] private TextMeshProUGUI answerText;
    [SerializeField] private string answerSelectedStatus = "Is that your final answer?";

    [Header("Backgrounds")]
    [SerializeField] private GameObject selectedBackground;
    [SerializeField] private GameObject answerBackground;

    private bool isSelected = false;
    private bool isDisabled = false;

    private Color normalTextColor;
    private Color normalLetterColor;
    private GameController gc;
    private SoundController sc;
    private List<AnswerButton> answerButtons;

    void Start()
    {
        gc = FindObjectOfType<GameController>();
        sc = FindObjectOfType<SoundController>();

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
        Answer correctAnswer = gc.CorrectAnswer;

        // Momentarily prevent the user from pressing other buttons
        DisableAll(true);

        // Give some feedback
        sc.PlayOneShot(finalAnswerClip);
        gc.StatusText = Icons.Hourglass;

        yield return new WaitForSeconds(gc.RevealAnswerDelay);

        // Indicate whether or not the chosen answer was correct
        if (answerValue == correctAnswer)
        {
            SetSelected(false);
            answerBackground.SetActive(true);

            StartCoroutine(gc.LoadNextQuestion());
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

            StartCoroutine(gc.EndGame());
        }
    }

    private void SetSelected(bool flag)
    {
        if (!isDisabled)
        {
            isSelected = flag;
            selectedBackground.SetActive(flag);

            if (flag)
            {
                answerButtons.ForEach(ab => ab.SetSelected(false));
                gc.StatusText = answerSelectedStatus;

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
