using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;
using TMPro;
using Utilities;
using Grammars.Common;

namespace Grammars
{
    public class GameOverGrammar : GrammarController
    {
        [Header("Events")]
        [Tooltip("Fired whenever the user says one of the play phrases.")]
        [SerializeField] private UnityEvent onPlayUtterance;
        [Tooltip("Fired whenever the user says one of the quit phrases.")]
        [SerializeField] private UnityEvent onQuitUtterance;

        [Header("UI")]
        [SerializeField] private GameOver gameOverUI;
        [SerializeField] private TextMeshProUGUI capsLockText;

        private bool capsLock = false;

        public override void Start()
        {
            base.Start();

            Actions.Add(Option.Play, onPlayUtterance.Invoke);
            Actions.Add(Option.Quit, onQuitUtterance.Invoke);
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
                    Actions[valueString]?.Invoke();
                }
                else if (keyString == Keys.CapsLock)
                {
                    HandleCapsLock(valueString);
                }
                else if (keyString == Keys.InputLetter)
                {
                    HandleLetterInput(valueString);
                }
            }
        }

        /// <summary>
        /// Toggle CapsLock.
        /// </summary>
        private void HandleCapsLock(string valueString)
        {
            bool result;
            if (bool.TryParse(valueString, out result))
            {
                capsLock = result;
                capsLockText.text = $"{Icons.Microphone} CapsLock: {(capsLock ? "On" : "Off")}";
            }
        }

        /// <summary>
        /// Insert the spoken letter into the text field.
        /// </summary>
        private void HandleLetterInput(string letter)
        {
            if (letter == SpecialLetters.Submit)
            {
                // Submit the form
                gameOverUI.OnSaveScore();
            }
            else if (letter == SpecialLetters.Backspace)
            {
                // Remove last letter
                if (gameOverUI.NameInput.Length > 0)
                {
                    gameOverUI.NameInput = gameOverUI.NameInput.Substring(0, gameOverUI.NameInput.Length - 1);
                }
            }
            else if (letter == SpecialLetters.Space)
            {
                // Insert a space
                gameOverUI.NameInput += " ";
            }
            else
            {
                // Insert a new letter
                if (capsLock)
                {
                    gameOverUI.NameInput += letter.ToUpper();
                }
                else
                {
                    gameOverUI.NameInput += letter.ToLower();
                }
            }
        }
    }
}