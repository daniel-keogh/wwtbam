using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;
using Grammars.Common;

namespace Grammars
{
    public class GameOverGrammar : GrammarController
    {
        [SerializeField] private GameOver gameOverUI;

        [Header("Events")]
        [Tooltip("Fired whenever the user says one of the play keywords.")]
        [SerializeField] private UnityEvent onPlayUtterance;

        [Tooltip("Fired whenever the user says one of the quit keywords.")]
        [SerializeField] private UnityEvent onQuitUtterance;

        private Dictionary<string, Action> actions = new Dictionary<string, Action>();

        public override void Start()
        {
            base.Start();

            actions.Add(Option.Play, () => onPlayUtterance?.Invoke());
            actions.Add(Option.Quit, () => onQuitUtterance?.Invoke());

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

                Debug.Log($"Key: {keyString}, Value: {valueString}");

                if (keyString == Keys.Option || keyString == Keys.Tutorial)
                {
                    actions[valueString]?.Invoke();
                }
                else if (keyString == Keys.InputLetter)
                {
                    HandleLetterInput(valueString);
                }
            }
        }

        /// <summary>
        /// Insert the spoken letter into the text field.
        /// </summary>
        private void HandleLetterInput(string letter)
        {
            switch (letter)
            {
                case SpecialLetters.Submit:
                    gameOverUI.OnSaveScore();
                    break;
                case SpecialLetters.Backspace:
                    if (gameOverUI.NameInput.Length > 0)
                        gameOverUI.NameInput = gameOverUI.NameInput.Substring(0, gameOverUI.NameInput.Length - 1);
                    break;
                case SpecialLetters.Space:
                    gameOverUI.NameInput += " ";
                    break;
                default:
                    gameOverUI.NameInput += letter;
                    break;
            }
        }
    }
}