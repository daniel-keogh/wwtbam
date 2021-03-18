using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using OpenTDB;
using Utilities;

/// <summary>
/// Singleton object for managing the game state.
/// </summary>
public class GameController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip correctAnswerClip;
    [SerializeField] private AudioClip wrongAnswerClip;

    [Header("Delays")]
    [Tooltip("Delay between showing the answer to the current question and loading the next question.")]
    [SerializeField] private float nextQuestionDelay = 3f;
    [Tooltip("Delay between showing the user they answered incorrectly and loading the GameOver scene.")]
    [SerializeField] private float gameOverDelay = 5f;
    [Tooltip("Delay before showing the user if they got the correct answer.")]
    [SerializeField] private float revealAnswerDelay = 4f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private string defaultStatus;

    [Header("Status Messages")]
    [SerializeField] private string correctAnswerStatus = "That's the right answer!";
    [SerializeField] private string incorrectAnswerStatus = "That's the wrong answer.";

    private SceneController sceneController;
    private SoundController soundController;
    private QuestionDisplay questionDisplay;
    private Question currentQuestion;
    private Answer correctAnswer;
    private List<string> allAnswers;
    private int numQuestions;
    private int currentQuestionNumber = 0;
    private bool didWalkAway = false;
    private List<int> prizes = new List<int>
    {
        0,
        500,
        1_000,
        2_000,
        5_000,
        10_000,
        20_000,
        50_000,
        75_000,
        150_000,
        250_000,
        500_000,
        1_000_000
    };
    private List<int> safetyNets = new List<int> { 1_000, 50_000 };

    public int CurrentWinnings => prizes[currentQuestionNumber];
    public Question CurrentQuestion => currentQuestion;
    public Answer CorrectAnswer => correctAnswer;
    public List<int> Prizes => prizes;
    public List<int> SafetyNets => safetyNets;
    public float RevealAnswerDelay => revealAnswerDelay;

    public string A => allAnswers[(int)Answer.A];
    public string B => allAnswers[(int)Answer.B];
    public string C => allAnswers[(int)Answer.C];
    public string D => allAnswers[(int)Answer.D];

    public string StatusText
    {
        get => statusText.text;
        set => statusText.text = value;
    }

    void Awake()
    {
        SetupSingleton();
    }

    void Start()
    {
        questionDisplay = FindObjectOfType<QuestionDisplay>();
        sceneController = FindObjectOfType<SceneController>();
        soundController = FindObjectOfType<SoundController>();

        if (SceneManager.GetActiveScene().name == SceneNames.GameScene)
        {
            numQuestions = prizes.Count - 1;
            NextQuestion();
        }
    }

    /// <summary>
    /// Loads the next question or ends the game if on the last question.
    /// </summary>
    public IEnumerator LoadNextQuestion()
    {
        // Give the player some feedback
        statusText.text = correctAnswerStatus;
        soundController.PlayOneShot(correctAnswerClip);

        yield return new WaitForSeconds(nextQuestionDelay);

        if (++currentQuestionNumber == numQuestions)
        {
            EndGame();
        }
        else
        {
            NextQuestion();
        }
    }

    /// <summary>
    /// Start the Game Over sequence.
    /// </summary>
    public IEnumerator EndGame()
    {
        // Give the player some feedback
        statusText.text = incorrectAnswerStatus;
        soundController.PlayOneShot(wrongAnswerClip);

        yield return new WaitForSeconds(gameOverDelay);

        sceneController.GameOver();
    }

    /// <summary>
    /// Calculates the user's final score in the game.
    /// </summary>
    public int GetFinalWinnings()
    {
        if (didWalkAway || CurrentWinnings == prizes.Max())
        {
            return CurrentWinnings;
        }

        foreach (var sn in safetyNets)
        {
            if (CurrentWinnings >= sn)
            {
                return sn;
            }
        }

        return 0;
    }

    /// <summary>
    /// Hides a given answer (e.g. When the 50/50 lifeline is used).
    /// </summary>
    public void HideAnswer(string ans)
    {
        int index = allAnswers.IndexOf(ans);
        allAnswers[index] = "";
        AnswerButton.DisableAnswer((Answer)index);

        questionDisplay.DisplayQuestion();
    }

    /// <summary>
    /// Quits the current playthrough.
    /// </summary>
    public void TakeTheMoney()
    {
        didWalkAway = true;
        sceneController.GameOver();
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Fetch and display the next question from the OpenTDB API.
    /// </summary>
    private void NextQuestion()
    {
        var request = new QuestionRequest
        {
            difficulty = GetDifficulty()
        };

        RequestHandler
           .GetQuestions(request)
           .Then(res =>
           {
               currentQuestion = res.results[0];

               ShuffleAnswers();

               questionDisplay.DisplayQuestion();
               AnswerButton.ResetAll();

               statusText.text = defaultStatus;
           })
           .Catch(err =>
           {
               Debug.LogError(err);
               statusText.text = err.Message;
           });
    }

    /// <summary>
    /// Determines the level of difficulty the next question ought to have.
    /// </summary>
    private string GetDifficulty()
    {
        if (CurrentWinnings >= safetyNets[1])
        {
            return Difficulty.Hard;
        }
        else if (CurrentWinnings >= safetyNets[0])
        {
            return Difficulty.Medium;
        }
        else
        {
            return Difficulty.Easy;
        }
    }

    /// <summary>
    /// Randomises the answers fetched from the API
    /// </summary>
    private void ShuffleAnswers()
    {
        allAnswers = new List<string>(currentQuestion.incorrect_answers);
        allAnswers.Add(currentQuestion.correct_answer);

        // Randomise the list of answers
        // Reference: https://stackoverflow.com/a/4262134
        allAnswers = allAnswers.OrderBy(a => Guid.NewGuid()).ToList();

        correctAnswer = (Answer)allAnswers.IndexOf(currentQuestion.correct_answer);
    }

    /// <summary>
    /// Turns this object into a singleton.
    /// </summary>
    private void SetupSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
