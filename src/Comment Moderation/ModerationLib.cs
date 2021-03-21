using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using System;
using System.IO;
using System.Text;

namespace Comment_Moderation
{
    public class ModerationLib
    {
        private static readonly string SubscriptionKey = "SAMPLE_KEY";
        private static readonly string Endpoint = "SAMPLE_ENDPOINT";

        public void ModerateText(string text)
        {
            var azClient = Authenticate();
            var azResult = CallAzureModerator(azClient, text);

            // TODO:
            // Check if the text contains a blocklisted word using List

            if (azResult.PII != null && (azResult.PII.Email.Count > 0 || azResult.PII.Phone.Count > 0))
            {
                // Don't post the comment and inform the user
                Console.WriteLine("Contains Personal Data");
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

        private Screen CallAzureModerator(ContentModeratorClient client, string text)
        {
            var textBytes = Encoding.UTF8.GetBytes(text);
            using var stream = new MemoryStream(textBytes);
            return client.TextModeration.ScreenText("text/plain", stream, language: "eng", autocorrect: false, pII: true, listId:null, true);
        }
    }
}
