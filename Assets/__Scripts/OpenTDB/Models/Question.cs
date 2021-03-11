using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenTDB
{
    /// <summary>
    /// Defines the structure of each question object returned from the OpenTDB API.
    /// </summary>
    [Serializable]
    public class Question : Model
    {
        public string category;
        public string type;
        public string difficulty;
        public string question;
        public string correct_answer;
        public List<string> incorrect_answers;
    }

    /// <summary>
    /// Defines how requests to the OpenTDB API should be structured. All
    /// fields are given default values and are therefore optional.
    /// </summary>
    [Serializable]
    public class QuestionRequest : Model
    {
        public int amount = 1;
        public string difficulty = Difficulty.Easy;
        public string type = QuestionType.MultipleChoice;
        public string encoding = Encoding.Default;
        public int category; // Random category
    }

    /// <summary>
    /// Response from OpenTDB when a request is made for a list of questions.
    /// </summary>
    [Serializable]
    public class QuestionResponse : Model
    {
        public int reponse_code;
        public List<Question> results;
    }
}
