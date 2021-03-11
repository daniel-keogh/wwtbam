using System;
using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using RSG;

namespace OpenTDB
{
    /// <summary>
    /// This class handles requests made to the Open Trivia DB API.
    /// </summary>
    public static class RequestHandler
    {
        private static readonly string BASE_URL = "https://opentdb.com/api.php";

        public static bool EnableDebug = false;

        public static IPromise<QuestionResponse> GetQuestions(QuestionRequest request)
        {
            return RestClient.Get<QuestionResponse>(new RequestHelper
            {
                Uri = BASE_URL,
                Params = new Dictionary<string, string> {
                    { "amount", request.amount.ToString() },
                    { "difficulty", request.difficulty },
                    { "type", request.type },
                    { "encoding", request.encoding },
                    { "category", request.category.ToString() },
                },
                EnableDebug = EnableDebug
            })
            .Catch(err => throw err as RequestException);
        }
    }
}
