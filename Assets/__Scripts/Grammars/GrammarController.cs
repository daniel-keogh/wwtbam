using UnityEngine;
using UnityEngine.Windows.Speech;

/// <summary>
/// This class defines the base functionality for all other GrammarController objects.
/// </summary>
public abstract class GrammarController : MonoBehaviour
{
    [Header("Grammar")]
    [Tooltip("The SRGS grammar file used by this object.")]
    [SerializeField] private string xmlFile;
    [SerializeField] private ConfidenceLevel confidence = ConfidenceLevel.Low;

    private GrammarRecognizer gr;

    public virtual void Start()
    {
        gr = new GrammarRecognizer($"{Application.streamingAssetsPath}/{xmlFile}.xml", confidence);
        gr.OnPhraseRecognized += OnPhraseRecognized;
        gr.Start();

        if (gr.IsRunning)
        {
            Debug.Log($"Loaded {xmlFile}.xml grammar.");
        }
    }

    void OnApplicationQuit() => Shutdown();

    void OnDisable() => Shutdown();

    private void Shutdown()
    {
        if (gr != null && gr.IsRunning)
        {
            Debug.Log($"Stopping grammar recognition for {xmlFile}.xml...");

            gr.OnPhraseRecognized -= OnPhraseRecognized;
            gr.Stop();
        }
    }

    /// <summary>
    /// Event emitted whenever a new phrase is picked up by the GrammarRecognizer.
    /// </summary>
    public abstract void OnPhraseRecognized(PhraseRecognizedEventArgs args);
}
