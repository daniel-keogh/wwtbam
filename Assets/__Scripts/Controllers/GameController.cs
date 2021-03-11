using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using OpenTDB;
using Utilities;

public class GameController : MonoBehaviour
{
    [Header("Delays")]
    [Tooltip("Delay between showing the answer to the current question and loading the next question.")]
    [SerializeField] private float nextQuestionDelay = 3f;
    [Tooltip("Delay between showing the user they answered incorrectly and loading the GameOver scene.")]
    [SerializeField] private float gameOverDelay = 5f;

    [Header("Prize Money")]
    [Tooltip("The list of prizes that can be one in the game.")]
    [SerializeField]
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

    [SerializeField]
    private List<int> safetyNets = new List<int> { 1_000, 50_000 };

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private string defaultStatus;

    private QuestionDisplay questionDisplay;
    private Question currentQuestion;
    private Answer correctAnswer;
    private List<string> allAnswers;
    private int numQuestions;
    private int currentQuestionNumber = 0;
    private bool didWalkAway = false;

    public int CurrentWinnings => prizes[currentQuestionNumber];

    public Question CurrentQuestion => currentQuestion;
    public Answer CorrectAnswer => correctAnswer;

    public List<int> Prizes => prizes;
    public List<int> SafetyNets => safetyNets;

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

        if (SceneManager.GetActiveScene().name == SceneNames.GameScene)
        {
            numQuestions = prizes.Count - 1;
            NextQuestion();
        }
    }

    public IEnumerator LoadNextQuestion()
    {
        statusText.text = "That's the right answer!";

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

    public IEnumerator EndGame()
    {
        statusText.text = "That's the wrong answer.";

        yield return new WaitForSeconds(gameOverDelay);

        SceneManager.LoadScene(SceneNames.GameOver);
    }

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

    public void TakeTheMoney()
    {
        didWalkAway = true;
        SceneManager.LoadScene(SceneNames.GameOver);
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

    private void NextQuestion()
    {
        // Fetch the next question from the OpenTDB API
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

    private void ShuffleAnswers()
    {
        allAnswers = new List<string>(currentQuestion.incorrect_answers);
        allAnswers.Add(currentQuestion.correct_answer);

        // Randomise the list of answers
        // Reference: https://stackoverflow.com/a/4262134
        allAnswers = allAnswers.OrderBy(a => Guid.NewGuid()).ToList();

        correctAnswer = (Answer)allAnswers.IndexOf(currentQuestion.correct_answer);
    }

    private void SetupSingleton()
    {
        // Check for any other objects of the same type
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            // Destroy the current object
            Destroy(gameObject);
        }
        else
        {
            // Persist across scenes
            DontDestroyOnLoad(gameObject);
        }
    }
}
