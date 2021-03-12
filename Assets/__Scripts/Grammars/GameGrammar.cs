using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using Grammars.Common;

namespace Grammars
{
    /// <summary>
    /// The grammar controller for the main game scene. 
    /// </summary>
    public class GameGrammar : GrammarController
    {
        [SerializeField] private GameObject quitConfirmationDialog;

        public delegate void AnswerSelectedEvent(Answer answer);
        public delegate void FinalAnswerEvent();
        public delegate void LifelineEvent(Lifeline lifeline);

        public static AnswerSelectedEvent OnAnswerSelected;
        public static FinalAnswerEvent OnFinalAnswer;
        public static LifelineEvent OnLifeline;

        private GameController gc;
        private Dictionary<string, Action> actions = new Dictionary<string, Action>();

        public override void Start()
        {
            base.Start();
            gc = FindObjectOfType<GameController>();

            actions.Add(Tutorial.Show, () => onShowTutorialUtterance?.Invoke());
            actions.Add(Tutorial.Hide, () => onHideTutorialUtterance?.Invoke());
        }

        public override void OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            SemanticMeaning[] meanings = args.semanticMeanings;

            foreach (var meaning in meanings)
            {
                string keyString = meaning.key.Trim();
                string valueString = meaning.values[0].Trim();

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
                        quitConfirmationDialog.SetActive(true);
                        break;
                    case Keys.QuitConfirmation:
                        HandleQuitConfirmation(valueString);
                        break;
                    case Common.Keys.Tutorial:
                        actions[valueString].Invoke();
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
                case Lifelines.AskTheAudience:
                    OnLifeline?.Invoke(Lifeline.AskTheAudience);
                    break;
                case Lifelines.FiftyFifty:
                    OnLifeline?.Invoke(Lifeline.FiftyFifty);
                    break;
                case Lifelines.PhoneAFriend:
                    OnLifeline?.Invoke(Lifeline.PhoneAFriend);
                    break;
                default:
                    break;
            }
        }

        private void HandleQuitConfirmation(string valueString)
        {
            if (bool.TryParse(valueString, out _))
            {
                Debug.Log("Taking the money...");
                gc.TakeTheMoney();
            }
            else
            {
                quitConfirmationDialog.SetActive(false);
            }
        }

        private static class Keys
        {
            public const string AnswerSelected = "selected";
            public const string FinalAnswer = "finalAnswer";
            public const string Lifeline = "lifeline";
            public const string TakeTheMoney = "takeMoney";
            public const string QuitConfirmation = "quitConfirmation";
        }

        private static class Lifelines
        {
            public const string AskTheAudience = "ask-audience";
            public const string FiftyFifty = "fifty-fifty";
            public const string PhoneAFriend = "phone-friend";
        }
    }
}
