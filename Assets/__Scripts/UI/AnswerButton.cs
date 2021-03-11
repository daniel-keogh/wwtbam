using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    [SerializeField] private Answer answerValue;

    [Header("Backgrounds")]
    [SerializeField] private GameObject selectedBackground;
    [SerializeField] private GameObject answerBackground;

    private bool isSelected = false;
    private bool isDisabled = false;

    private List<AnswerButton> answerButtons;
    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();

        // Maintain a list of the other buttons
        answerButtons = FindObjectsOfType<AnswerButton>()
            .Where(ab => ab != this)
            .ToList();
    }

    void OnEnable()
    {
        GameGrammarController.OnAnswerSelected += OnAnswerSelectedEvent;
        GameGrammarController.OnFinalAnswer += OnFinalAnswerEvent;
    }

    void OnDisable()
    {
        GameGrammarController.OnAnswerSelected -= OnAnswerSelectedEvent;
        GameGrammarController.OnFinalAnswer -= OnFinalAnswerEvent;
    }

    public void OnClick()
    {
        if (isSelected)
        {
            CheckAnswer();
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
        if (isSelected)
        {
            CheckAnswer();
        }
    }

    private void CheckAnswer()
    {
        Answer correctAnswer = gameController.CorrectAnswer;

        // Momentarily prevent the user from pressing other buttons
        DisableAll(true);

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
                }
            });

            StartCoroutine(gameController.EndGame());
        }

        DisableAll(false);
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
