using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Comment_Moderation
{
    public class ModerationLib
    {
        #region Credentials

        private static readonly string SubscriptionKey = "SAMPLE_KEY";
        private static readonly string Endpoint = "SAMPLE_ENDPOINT";

        #endregion Credentials

        private ContentModeratorClient client;

        public ModerationLib()
        {
            client = Authenticate();
        }

        #region Moderating Text

        public void ModerateText(string text, string listId)
        {
            var azResult = CallAzureModerator(text, listId);
            if (azResult.PII != null && (azResult.PII.Email.Count > 0 || azResult.PII.Phone.Count > 0))
            {
                // Don't post the comment and inform the user
                Console.WriteLine("Contains Personal Data");
            }
            else if (azResult.Terms != null)
            {
                Console.WriteLine("Contains Blocked words from the list");
            }
            else if (azResult.Classification.ReviewRecommended == true)
            {
                
                if (azResult.Classification.Category1.Score > 0.9)
                {
                    // Automatically reject comments with greater confidence in inappropriate content.
                    // Category 1 refers to potential presence of language that may be considered sexually explicit or adult in certain situations
                    // There are a total of 3 categories. Documentaion: https://docs.microsoft.com/en-us/azure/cognitive-services/content-moderator/text-moderation-api#classification
                    Console.WriteLine("Rejected the comment");
                }
                else
                {
                    // Make use of manual review queue to filter these comments
                    Console.WriteLine("Comment held for review");
                }
            }
            else
            {
                Console.WriteLine("Comment Published");
            }
        }

        private ContentModeratorClient Authenticate()
        {
            var client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(SubscriptionKey))
            {
                Endpoint = Endpoint
            };
            return client;
        }

        private Screen CallAzureModerator(string text, string listId)
        {
            var textBytes = Encoding.UTF8.GetBytes(text);
            using var stream = new MemoryStream(textBytes);
            return client.TextModeration.ScreenText("text/plain", stream, language: "eng", autocorrect: false, pII: true, listId:listId, classify: true);
        }

        #endregion Moderating Text

        #region Managing Lists

        public string CreateTermList(string listName, string listDesc)
        {
            var body = new Body(listName, listDesc);
            var list = client.ListManagementTermLists.Create("application/json", body);
            var listId = list.Id.Value.ToString();
            return listId;
        }

        public void AddToTermsList(string listId, List<string> terms, int throttleRate = 3000, string lang = "eng")
        {
            foreach(var term in terms)
            {
                var result = client.ListManagementTerm.AddTerm(listId, term, lang);
                Thread.Sleep(throttleRate);
            }
        }

        public List<string> GetAllTerms(string listId, string lang = "eng")
        {
            var termsData = client.ListManagementTerm.GetAllTerms(listId, lang).Data;
            List<string> termsList = new List<string>();
            foreach (var term in termsData.Terms)
            {
                termsList.Add(term.Term);
            }
            return termsList;
        }

        public void DeleteTerm(string listId, string term, string lang = "eng")
            => client.ListManagementTerm.DeleteTerm(listId, term, lang);

        public void DeleteAllTerms(string listId, string lang = "eng")
            => client.ListManagementTerm.DeleteAllTerms(listId, lang);

        public void DeleteTermList(string listId)
            => client.ListManagementTermLists.Delete(listId);

        #endregion Managing Lists

    }
}
