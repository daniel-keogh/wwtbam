using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;

namespace Grammars
{
    /// <summary>
    /// This class defines the base functionality for all other GrammarController objects.
    /// </summary>
    public abstract class GrammarController : MonoBehaviour
    {
        [Header("Grammar")]
        [Tooltip("The SRGS grammar file used by this object.")]
        [SerializeField] private string xmlFile;
        [SerializeField] private ConfidenceLevel confidence = ConfidenceLevel.Low;

        [Header("Tutorial")]
        [Tooltip("Fired whenever the user asks to view the tutorial screen.")]
        [SerializeField] protected UnityEvent onShowTutorialUtterance;

        [Tooltip("Fired whenever the user asks to hide the tutorial screen.")]
        [SerializeField] protected UnityEvent onHideTutorialUtterance;

        private GrammarRecognizer gr;

        public virtual void Start()
        {
            gr = new GrammarRecognizer($"{Application.streamingAssetsPath}/{xmlFile}", confidence);
            gr.OnPhraseRecognized += OnPhraseRecognized;
            gr.Start();

            if (gr.IsRunning)
            {
                Debug.Log($"Loaded {xmlFile} grammar.");
            }
        }

        void OnApplicationQuit() => Shutdown();

        void OnDisable() => Shutdown();

        private void Shutdown()
        {
            if (gr != null && gr.IsRunning)
            {
                Debug.Log($"Stopping grammar recognition for {xmlFile}...");

                gr.OnPhraseRecognized -= OnPhraseRecognized;
                gr.Stop();
            }
        }

        /// <summary>
        /// Event emitted whenever a new phrase is picked up by the GrammarRecognizer.
        /// </summary>
        public abstract void OnPhraseRecognized(PhraseRecognizedEventArgs args);
    }
}
