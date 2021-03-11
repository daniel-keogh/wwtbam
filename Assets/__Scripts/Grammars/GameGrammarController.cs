using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

/// <summary>
/// The grammar controller for the main game scene. 
/// </summary>
public class GameGrammarController : GrammarController
{
    public delegate void AnswerSelectedEvent(Answer answer);
    public delegate void FinalAnswerEvent();
    public delegate void LifelineEvent(Lifeline lifeline);

    public static AnswerSelectedEvent OnAnswerSelected;
    public static FinalAnswerEvent OnFinalAnswer;
    public static LifelineEvent OnLifeline;

    private GameController gc;

    public override void Start()
    {
        base.Start();
        gc = FindObjectOfType<GameController>();
    }

    public override void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        SemanticMeaning[] meanings = args.semanticMeanings;

        foreach (var meaning in meanings)
        {
            string keyString = meaning.key.Trim();
            string valueString = meaning.values[0].Trim();

            Debug.Log(keyString + " " + valueString);

            switch (keyString)
            {
                case Keys.AnswerSelected:
                    HandleAnswerSelected(valueString);
                    break;
                case Keys.FinalAnswer:
                    HandleFinalAnswer();
                    break;
                case Keys.Lifeline:
                    HandleLifeline(valueString);
                    break;
                case Keys.TakeTheMoney:
                    HandleTakeTheMoney();
                    break;
                default:
                    break;
            }
        }
    }

    private void HandleAnswerSelected(string valueString)
    {
        Debug.Log($"Selecting answer {valueString}...");

        switch (valueString)
        {
            case "A":
                OnAnswerSelected?.Invoke(Answer.A);
                break;
            case "B":
                OnAnswerSelected?.Invoke(Answer.B);
                break;
            case "C":
                OnAnswerSelected?.Invoke(Answer.C);
                break;
            case "D":
                OnAnswerSelected?.Invoke(Answer.D);
                break;
            default:
                break;
        }
    }

    private void HandleFinalAnswer()
    {
        Debug.Log($"Final answer...");
        OnFinalAnswer?.Invoke();
    }

    private void HandleLifeline(string valueString)
    {
        Debug.Log($"Using lifeline " + valueString);

        switch (valueString)
        {
            case LifelineValues.AskTheAudience:
                OnLifeline?.Invoke(Lifeline.AskTheAudience);
                break;
            case LifelineValues.FiftyFifty:
                OnLifeline?.Invoke(Lifeline.FiftyFifty);
                break;
            case LifelineValues.PhoneAFriend:
                OnLifeline?.Invoke(Lifeline.PhoneAFriend);
                break;
            default:
                break;
        }
    }

    private void HandleTakeTheMoney()
    {
        Debug.Log("Taking the money...");
        gc.TakeTheMoney();
    }

    private static class Keys
    {
        public const string AnswerSelected = "selected";
        public const string FinalAnswer = "finalAnswer";
        public const string Lifeline = "lifeline";
        public const string TakeTheMoney = "takeMoney";
    }

    private static class LifelineValues
    {
        public const string AskTheAudience = "ask-audience";
        public const string FiftyFifty = "fifty-fifty";
        public const string PhoneAFriend = "phone-friend";
    }
}
