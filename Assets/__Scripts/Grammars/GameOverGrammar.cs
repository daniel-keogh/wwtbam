﻿using System;
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
        [Header("Events")]
        [Tooltip("Fired whenever the user says one of the play phrases.")]
        [SerializeField] private UnityEvent onPlayUtterance;
        [Tooltip("Fired whenever the user says one of the quit phrases.")]
        [SerializeField] private UnityEvent onQuitUtterance;

        [Header("UI")]
        [SerializeField] private GameOver gameOverUI;

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