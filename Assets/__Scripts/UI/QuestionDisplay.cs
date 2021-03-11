using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using TMPro;

public class QuestionDisplay : MonoBehaviour
{
    [Header("Question Text Field")]
    [SerializeField] private TextMeshProUGUI questionText;

    [Header("Answer Text Fields")]
    [SerializeField] private TextMeshProUGUI aText;
    [SerializeField] private TextMeshProUGUI bText;
    [SerializeField] private TextMeshProUGUI cText;
    [SerializeField] private TextMeshProUGUI dText;

    private GameController gc;

    void Start()
    {
        gc = FindObjectOfType<GameController>();
    }

    public void DisplayQuestion()
    {
        // Escape HTML entities that get returned from the API
        // Reference: https://stackoverflow.com/a/13492748
        questionText.text = WebUtility.HtmlDecode(gc.CurrentQuestion.question);

        aText.text = WebUtility.HtmlDecode(gc.A);
        bText.text = WebUtility.HtmlDecode(gc.B);
        cText.text = WebUtility.HtmlDecode(gc.C);
        dText.text = WebUtility.HtmlDecode(gc.D);
    }
}
