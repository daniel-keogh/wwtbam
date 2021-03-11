using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;

/// <summary>
/// Grammar controller used by the menu scenes.
/// </summary>
public class MenuGrammarController : GrammarController
{
    [Header("Events")]
    [Tooltip("Fired whenever the user says one of the play keywords.")]
    [SerializeField] private UnityEvent onPlayUtterance;
    [Tooltip("Fired whenever the user says one of the quit keywords.")]
    [SerializeField] private UnityEvent onQuitUtterance;

    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    public override void Start()
    {
        base.Start();

        actions.Add(Options.Play, () => onPlayUtterance?.Invoke());
        actions.Add(Options.Quit, () => onQuitUtterance?.Invoke());
    }

    public override void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        SemanticMeaning[] meanings = args.semanticMeanings;

        foreach (var meaning in meanings)
        {
            string keyString = meaning.key.Trim();
            string valueString = meaning.values[0].Trim();

            if (keyString == Keys.Option)
            {
                actions[valueString]?.Invoke();
            }
        }
    }

    private static class Keys
    {
        public const string Option = "option";
    }

    private static class Options
    {
        public const string Play = "play-game";
        public const string Quit = "quit-game";
    }
}
