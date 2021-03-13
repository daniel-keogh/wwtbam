using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;
using Grammars.Common;

namespace Grammars
{
    /// <summary>
    /// Grammar controller used by the main menu scene.
    /// </summary>
    public class MenuGrammar : GrammarController
    {
        [Header("Events")]
        [Tooltip("Fired whenever the user says one of the play phrases.")]
        [SerializeField] private UnityEvent onPlayUtterance;
        [Tooltip("Fired whenever the user asks to see the leaderboard.")]
        [SerializeField] private UnityEvent onShowLeaderboardUtterance;
        [Tooltip("Fired whenever the user asks to hide the leaderboard.")]
        [SerializeField] private UnityEvent onHideLeaderboardUtterance;
        [Tooltip("Fired whenever the user says one of the quit phrases.")]
        [SerializeField] private UnityEvent onQuitUtterance;

        public override void Start()
        {
            base.Start();

            Actions.Add(Option.Play, onPlayUtterance.Invoke);
            Actions.Add(Option.Quit, onQuitUtterance.Invoke);
            Actions.Add(Leaderboard.Show, onShowLeaderboardUtterance.Invoke);
            Actions.Add(Leaderboard.Hide, onHideLeaderboardUtterance.Invoke);
        }

        public override void OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            SemanticMeaning[] meanings = args.semanticMeanings;

            foreach (var meaning in meanings)
            {
                string keyString = meaning.key.Trim();
                string valueString = meaning.values[0].Trim();

                Debug.Log($"Key: {keyString}, Value: {valueString}");

                if (
                    keyString == Keys.Option ||
                    keyString == Keys.Leaderboard ||
                    keyString == Keys.Tutorial
                )
                {
                    Actions[valueString]?.Invoke();
                }
            }
        }
    }
}