# Comment moderation using Azure cognitive services

This is a console application that demonstrates how to moderate comments using [Azure Content Moderator](https://docs.microsoft.com/en-us/azure/cognitive-services/content-moderator/). I have written blog posts that provide further details on this.

#### Associated Blog posts

 1. [Moderate comments on your website using Azure Content Moderator](https://kolappan.dev/2021/03/21/moderate-comments-using-azure-content-moderator)
 2. Creating & Managing custom blocklists in the Azure Content Moderator service
 3. Using the review system within Azure for Manual reviews

#### Functions covered in this POC Console App

- [x] Checking if a text contains personal data
- [x] Adult or explicit content
   - [x] Auto blocking comments where Azure identifies explict content with high confidence
   - [ ] Marking comments with low confidence for manual review
   - [ ] Manual review integration
- [x] Custom block list
   - [x] Block comments that contain words from a custom blocklist
   - [x] Creating & deleting custom block list
   - [x] Adding & removing words from the block list

#### Note

To view the main code that interacts with the Azure service head over to the [ModeratorLib class](https://github.com/kolappannathan/comment-moderation-using-azure/blob/main/src/Comment%20Moderation/ModerationLib.cs).
