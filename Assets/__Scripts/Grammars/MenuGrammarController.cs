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
    public class MenuGrammarController : GrammarController
    {
        [Header("Events")]
        [Tooltip("Fired whenever the user says one of the play keywords.")]
        [SerializeField] private UnityEvent onPlayUtterance;

        [Tooltip("Fired whenever the user asks to see the leaderboard.")]
        [SerializeField] private UnityEvent onShowLeaderboardUtterance;

        [Tooltip("Fired whenever the user asks to hide the leaderboard.")]
        [SerializeField] private UnityEvent onHideLeaderboardUtterance;

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

            actions.Add(Leaderboard.Show, () => onShowLeaderboardUtterance?.Invoke());
            actions.Add(Leaderboard.Hide, () => onHideLeaderboardUtterance?.Invoke());
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
                    actions[valueString]?.Invoke();
                }
            }
        }
    }
}